using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerIdleState : EntityIdleState
    {
        public PlayerIdleState(
            Player player,
            AnimatorController animatorController,
            Ball ball,
            Mover mover,
            CollisionHandler collisionHandler,
            Collider squadZone,
            Collider collider,
            Rigidbody rigidbody,
            PlayerConfig playerConfig,
            List<Entity> teammates)
            : base(
                animatorController,
                ball,
                mover,
                collisionHandler,
                squadZone,
                collider,
                rigidbody,
                player,
                playerConfig,
                teammates)
        {
        }

        protected override void SwitchToMove()
        {
            StateSwitcher.SwitchState<PlayerMoveState>();
        }

        protected override void SwitchToDodge()
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
    }
}