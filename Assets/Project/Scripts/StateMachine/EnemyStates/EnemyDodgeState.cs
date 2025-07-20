using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyDodgeState : EntityDodgeState
    {
        public EnemyDodgeState(AnimatorController animatorController, CollisionHandler collisionHandler, HitDetector hitDetector, Collider collider)
            : base(animatorController, collisionHandler, hitDetector, collider) { }

        protected override void OnJumpFinished()
        {
            StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }
    }
}