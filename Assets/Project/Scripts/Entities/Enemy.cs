using System;
using System.Collections.Generic;
using Project.Scripts.Services.AudioService;
using Project.Scripts.Services.EffectService;
using Project.Scripts.StateMachine;
using Project.Scripts.StateMachine.EnemyStates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public class Enemy : Entity, IDestoyable<Enemy>
    {
        [SerializeField] private EnemyConfig _enemyConfig;
    
        private List<IState> _enemyStates = new();
    
        public event Action<Enemy> OnDestroyed;

        public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
        {
            base.Initialize(squadZone, teammates, ball);
            BallThrower.Initialize(_enemyConfig);
        
            foreach (var state in _enemyStates)
            {
                if (state is IDisposable disposable)
                    disposable.Dispose();
            }
        
            _enemyStates.Clear();
        
            _enemyStates = new List<IState>
            {
                new EnemyPrepareState(this, AnimatorController, TargetScanner, Teammates),
                new EnemyCelebrateState(this, AnimatorController,BallHolder, BallThrower, CollisionHandler, Teammates),
                new EnemyIdleState(this,AnimatorController, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyConfig, Teammates),
                new EnemyMoveState(this, AnimatorController,Teammates, _enemyConfig, CollisionHandler, SquadZone, BallHolder, Collider, Mover),
                new EnemyDodgeReadyState(this, AnimatorController, ball, Mover, SquadZone, Rigidbody, _enemyConfig),
                new EnemyAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, _enemyConfig),
                new EnemyDodgeState(AnimatorController, CollisionHandler, HitDetector, Collider),
                new EnemyDeathState(AnimatorController, CollisionHandler, Collider, BallHolder, BallThrower)
            };
        
            StateMachine = new StateMaсhine(_enemyStates);

            foreach (var state in _enemyStates)
                state.Initialize(StateMachine);

            Reset();
        }
    
        [Button]
        protected override async void HandleLostHealth()
        {
            StateMachine.SwitchState<EnemyDeathState>();
            HealthCanvas.gameObject.SetActive(false);
            EffectID.Death.PlayEffect(transform);
            AudioID.Dead.PlayOneShot();
        
            await AnimatorController.Death();
            await HideEntity();

            Die();
        }

        public override void Celebrate()
        {
            StateMachine.SwitchState<EnemyCelebrateState>();
            BallHolder.LostBall();
        }

        [Button]
        public override void Die()
        {
            base.Die();
            OnDestroyed?.Invoke(this);
        }
    }
}