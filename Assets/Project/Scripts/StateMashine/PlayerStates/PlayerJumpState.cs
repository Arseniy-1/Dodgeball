using UnityEngine;

public class PlayerJumpState : EntityJumpState
{
    public PlayerJumpState(
        AnimatorController animatorController,
        CollisionHandler collisionHandler,
        HitCheker hitCheker,
        Collider collider)
        : base(animatorController, collisionHandler, hitCheker, collider)
    {
    }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<PlayerDodgeState>();
    }
}