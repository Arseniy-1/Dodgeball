using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services.AudioServiceSystem;
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
    
        public event Action<Enemy> OnDestroyed;
        
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
        
        [Button]
        protected override async UniTaskVoid HandleLostHealth(CancellationToken token)
        {
            StateMachine.SwitchState<EnemyDeathState>();
            HealthCanvas.gameObject.SetActive(false);
            EffectID.Death.PlayEffect(transform);
            AudioID.Dead.PlayOneShot();
        
            await AnimatorController.Death();
            await HideEntity(token);

            Die();
        }

        protected override List<IState> CreateStates()
        {
            return new List<IState>
            {
                new EnemyPrepareState(this, AnimatorController, TargetScanner, Teammates),
                new EnemyCelebrateState(this, AnimatorController, BallHolder, BallThrower, CollisionHandler, Teammates),
                new EnemyIdleState(this, AnimatorController, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyConfig, Teammates),
                new EnemyMoveState(this, AnimatorController, Teammates, _enemyConfig, CollisionHandler, SquadZone, BallHolder, Collider, Mover),
                new EnemyDodgeReadyState(this, AnimatorController, Ball, Mover, SquadZone, Rigidbody, _enemyConfig),
                new EnemyAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, _enemyConfig),
                new EnemyDodgeState(AnimatorController, CollisionHandler, HitDetector, Collider),
                new EnemyDeathState(AnimatorController, CollisionHandler, Collider, BallHolder, BallThrower),
            };
        }
        
        protected override EntityConfig GetConfig()
        {
            return _enemyConfig;
        }
    }
}