using UnityEngine;

public class EnemyDodgeState : EntityDodgeState
{
    public EnemyDodgeState(AnimatorController animatorController, CollisionHandler collisionHandler, HitCheker hitChecker, Collider collider)
        : base(animatorController, collisionHandler, hitChecker, collider) { }

    protected override void OnJumpFinished()
    {
        StateSwitcher.SwitchState<EnemyDodgeReadyState>();
    }
}