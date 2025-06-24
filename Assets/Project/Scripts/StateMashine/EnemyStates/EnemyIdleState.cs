using UnityEngine;

public class EnemyIdleState : EntityIdleState
{
    public EnemyIdleState(Enemy enemy,
        AnimatorController animatorController, Ball ball, Mover mover, CollisionHandler collisionHandler,
        Collider squadZone, Collider collider, Rigidbody rigidbody, EnemyStats enemyStats)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, enemy, enemyStats)
    {
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
        {
            StateSwitcher.SwitchState<EnemyMoveState>();
        }
        else
        {
            StateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }
}