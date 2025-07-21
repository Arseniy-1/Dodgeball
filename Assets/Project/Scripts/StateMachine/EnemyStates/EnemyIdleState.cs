using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;
using Project.Scripts.Entities;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyIdleState : EntityIdleState
    {
        public EnemyIdleState(Enemy enemy,
            AnimatorController animatorController, Ball ball, Mover mover, CollisionHandler collisionHandler,
            Collider squadZone, Collider collider, Rigidbody rigidbody, EnemyConfig enemyConfig, List<Entity> teammates)
            : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, enemy, enemyConfig, teammates) { }

        protected override void SwitchToMove()
        {
            StateSwitcher.SwitchState<EnemyMoveState>();
        }

        protected override void SwitchToDodge()
        {
            StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }
    }
}