using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDodgeState : EntityDodgeState
    {
        public PlayerDodgeState(AnimatorController animatorController, CollisionHandler collisionHandler, HitDetector hitDetector, Collider collider)
            : base(animatorController, collisionHandler, hitDetector, collider) { }

        protected override void OnJumpFinished()
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
    }
}