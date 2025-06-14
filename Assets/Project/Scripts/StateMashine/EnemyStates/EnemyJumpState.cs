using UnityEngine;

public class EnemyJumpState : EntityJumpState
{
    public EnemyJumpState(
        AnimatorController animatorController,
        GroundChecker groundChecker,
        CollisionHandler collisionHandler,
        Collider collider)
        : base(animatorController, groundChecker, collisionHandler, collider)
    {
    }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<EnemyDodgeState>();
    }
}