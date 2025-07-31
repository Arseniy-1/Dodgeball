using System;
using System.Collections.Generic;
using Project.Scripts.StateMachine;
using Project.Scripts.StateMachine.EnemyStates;
using Sirenix.OdinInspector;

namespace Project.Scripts.Entities
{
    public class Enemy : Entity, IDestoyable<Enemy>
    {
        public event Action<Enemy> Destroyed;

        public override void Celebrate()
        {
            StateMachine.SwitchState<EnemyCelebrateState>();
        }

        [Button]
        public override void Die()
        {
            base.Die();
            Destroyed?.Invoke(this);
        }

        protected override List<IState> CreateStates()
        {
            var dataHolder = new StateDataHolder(this, CollisionHandler, Collider, SquadZone,
                Rigidbody, AnimatorController, BallHolder, TargetScanner,
                TargetProvider, Teammates, BallThrower, Mover, EntityConfig, HitDetector);

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

        protected override void SwitchToDeathState()
        {
            StateMachine.SwitchState<EnemyDeathState>();
        }
    }
}