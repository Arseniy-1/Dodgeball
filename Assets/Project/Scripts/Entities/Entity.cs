using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine;
using Project.Scripts.UI.Canvases;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        [field: SerializeField] protected BallThrower BallThrower { get; private set; }
        [field: SerializeField] protected BallHolder BallHolder { get; private set; }
        [field: SerializeField] protected CollisionHandler CollisionHandler { get; private set; }
        [field: SerializeField] protected TargetScanner TargetScanner { get; private set; }
        [field: SerializeField] protected Mover Mover { get; private set; }
        [field: SerializeField] protected Health.Health Health { get; private set; }
        [field: SerializeField] protected List<Entity> Teammates { get; private set; }
        [field: SerializeField] protected Animator Animator { get; private set; }
        [field: SerializeField] protected HitDetector HitDetector { get; private set; }
        [field: SerializeField] protected HealthCanvas HealthCanvas { get; private set; }
        [field: SerializeField] protected Ball Ball { get; private set; }

        protected TargetProvider TargetProvider { get; private set; } = new ();
        protected Collider SquadZone { get; private set; }
        protected Collider Collider { get; private set; }
        protected Rigidbody Rigidbody { get; private set; }
        protected AnimatorController AnimatorController { get; private set; }
        protected StateMaсhine StateMachine { get; private set; }

        private void OnEnable()
        {
            Health.LostHealth += HandleLostHealth;
        }

        private void OnDisable()
        {
            Health.LostHealth -= HandleLostHealth;
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }
        
        public virtual void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
        {
            Collider = GetComponent<Collider>();
            Rigidbody = GetComponent<Rigidbody>();
            Teammates = teammates;
            SquadZone = squadZone;
            Health.Initialize(CollisionHandler);

            if (Animator != null)
                AnimatorController = new AnimatorController(Animator);
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
    
        protected abstract void HandleLostHealth();
    
        protected async UniTask HideEntity()
        {
            float duration = 1.5f;
            float elapsed = 0f;
            Vector3 start = transform.position;
            Vector3 target = start + Vector3.down * 2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transform.position = Vector3.Lerp(start, target, t);
                await UniTask.Yield();
            }

            transform.position = target;
        }

        protected void CreateStateMachine(List<IState> states)
        {
            StateMachine = new StateMaсhine(states);
        }
    }
}