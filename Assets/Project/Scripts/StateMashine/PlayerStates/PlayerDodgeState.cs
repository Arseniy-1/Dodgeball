using UnityEngine;

public class PlayerDodgeState : EntityDodgeState
{
    public PlayerDodgeState(AnimatorController animatorController, CollisionHandler collisionHandler, HitCheker hitChecker, Collider collider)
        : base(animatorController, collisionHandler, hitChecker, collider) { }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<PlayerDodgeReadyState>();
    }
}