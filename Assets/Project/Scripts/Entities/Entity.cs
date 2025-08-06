using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.HealthSystem;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.Ball;
using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.StateMachine;
using Project.Scripts.UI.Canvases;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        
        private CancellationTokenSource _cancellationTokenSource;
        private List<IState> _states = new ();

        [field: SerializeField] protected BallThrower BallThrower { get; private set; }
        [field: SerializeField] protected BallHolder BallHolder { get; private set; }
        [field: SerializeField] protected CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] protected TargetScanner TargetScanner { get; private set; }
        [field: SerializeField] protected Mover Mover { get; private set; }
        [field: SerializeField] protected Health Health { get; private set; }
        [field: SerializeField] protected List<Entity> Teammates { get; private set; }
        [field: SerializeField] protected Animator Animator { get; private set; }
        [field: SerializeField] protected HitDetector HitDetector { get; private set; }
        [field: SerializeField] protected HealthCanvas HealthCanvas { get; private set; }
        [field: SerializeField] protected EntityConfig EntityConfig { get; private set; }

        protected TargetProvider TargetProvider { get; private set; } = new ();
        protected Collider SquadZone { get; private set; }
        protected Collider Collider { get; private set; }
        protected Rigidbody Rigidbody { get; private set; }
        protected AnimatorController AnimatorController { get; private set; }
        protected StateMaсhine StateMachine { get; private set; }
        
        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Health.LostHealth += OnLostHealth;
        }

        private void OnDisable()
        {
            _cancellationTokenSource.Cancel();
            Health.LostHealth -= OnLostHealth;
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }
        
        public void Initialize(Collider squadZone, List<Entity> teammates)
        {
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();
            SquadZone = squadZone;
            Teammates = teammates;
            Health.Initialize(CollisionHandler);

            BallThrower.Initialize(EntityConfig);
            
            if (Animator != null)
                AnimatorController = new AnimatorController(Animator);
            
            foreach (var state in _states)
            {
                if (state is IDisposable disposable)
                    disposable.Dispose();
            }
        
            _states.Clear();

            _states = CreateStates();
            
            CreateStateMachine(_states);

            foreach (var state in _states)
                state.Initialize(StateMachine);

            Reset();
        }

        public virtual void Reset()
        {
            CollisionHandler.enabled = true;
            Collider.enabled = true;
            BallHolder.LostBall();
            HealthCanvas.gameObject.SetActive(true);
        }
    
        public abstract void Celebrate();
    
        [Button]
        public virtual void Die()
        {
            StateMachine.Dispose();
            AnimatorController.Dispose();
            Health.Reset();
        }

        protected abstract List<IState> CreateStates(); 
        protected abstract void SwitchToDeathState(); 
        
        private async UniTaskVoid HandleLostHealth(CancellationToken token)
        {
            SwitchToDeathState();
            HealthCanvas.gameObject.SetActive(false);
            EffectID.Death.PlayEffect(transform);
            AudioID.Dead.PlayOneShot();

            await AnimatorController.Death();
            await HideEntity(token);

            Die();
        }
    
        private async UniTask HideEntity(CancellationToken token)
        {
            float duration = 1.5f;
            float elapsed = 0f;
            Vector3 start = transform.position;
            Vector3 target = start + Vector3.down * 2f;

            while (elapsed < duration && token.IsCancellationRequested == false) 
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transform.position = Vector3.Lerp(start, target, t);
                await UniTask.Yield();
            }

            transform.position = target;
        }

        private void CreateStateMachine(List<IState> states)
        {
            StateMachine = new StateMaсhine(states);
        }

        private void OnLostHealth()
        {
            HandleLostHealth(_cancellationTokenSource.Token).Forget();
        }
    }
}