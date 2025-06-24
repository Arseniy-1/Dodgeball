using UnityEngine;

public class EnemyMoveState : EntityMoveState
{
    private readonly Enemy _enemy;

    public EnemyMoveState(Enemy enemy, AnimatorController animatorController, EnemyStats enemyStats,
        CollisionHandler collisionHandler, Collider squadZone,
        BallHolder ballHolder, Ball ball, Collider collider, Mover mover)
        : base(enemy, animatorController, collisionHandler, squadZone, ballHolder, ball, collider, enemyStats, mover)
    {
        _enemy = enemy;
    }

    protected override void OnBallDetected(Ball ball)
    {
        BallHolder.EquipBall(ball);
        StateSwitcher.SwitchState<EnemyAttackState>();
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone != SquadZone)
            StateSwitcher.SwitchState<EnemyDodgeState>();
    }

    protected override void HandleBallTaken(Entity entity)
    {
        if (entity == _enemy) return;

        Vector3 closestPoint = SquadZone.ClosestPoint(entity.transform.position);

        if (closestPoint == entity.transform.position)
            StateSwitcher.SwitchState<EnemyIdleState>();
        else
            StateSwitcher.SwitchState<EnemyDodgeState>();
    }
}