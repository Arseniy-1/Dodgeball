using UnityEngine;

public class PlayerJumpState : EntityJumpState
{
    public PlayerJumpState(
        AnimatorController animatorController,
        GroundChecker groundChecker,
        CollisionHandler collisionHandler,
        Collider collider)
        : base(animatorController, groundChecker, collisionHandler, collider)
    {
    }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<PlayerDodgeState>();
    }
}