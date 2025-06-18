using UnityEngine;

public class EnemyJumpState : EntityJumpState
{
    public EnemyJumpState(
        AnimatorController animatorController,
        CollisionHandler collisionHandler,
        HitCheker hitCheker,
        Collider collider)
        : base(animatorController, collisionHandler, hitCheker, collider)
    {
    }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<EnemyDodgeState>();
    }
}