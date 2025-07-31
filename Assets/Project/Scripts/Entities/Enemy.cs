using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.StateMachine;
using Project.Scripts.StateMachine.EnemyStates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public class Enemy : Entity, IDestoyable<Enemy>
    {
        [SerializeField] private EnemyConfig _enemyConfig;

        public event Action<Enemy> Destroyed;

        public override void Celebrate()
        {
            StateMachine.SwitchState<EnemyCelebrateState>();
            BallHolder.LostBall();
        }

        [Button]
        public override void Die()
        {
            base.Die();
            Destroyed?.Invoke(this);
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
            var dataHolder = new StateDataHolder(this, CollisionHandler, Collider, SquadZone,
                Rigidbody, AnimatorController, BallHolder, TargetScanner,
                TargetProvider, Teammates, BallThrower, Mover, _enemyConfig, HitDetector);

            return new List<IState>
            {
                new EnemyPrepareState(dataHolder),
                new EnemyCelebrateState(dataHolder),
                new EnemyIdleState(dataHolder),
                new EnemyMoveState(dataHolder),
                new EnemyDodgeReadyState(dataHolder),
                new EnemyAttackState(dataHolder),
                new EnemyDodgeState(dataHolder),
                new EnemyDeathState(dataHolder),
            };
        }

        protected override EntityConfig GetConfig()
        {
            return _enemyConfig;
        }
    }
}