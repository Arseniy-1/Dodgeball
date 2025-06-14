using UnityEngine;

public class EnemyIdleState : EntityIdleState
{
    private readonly Enemy _enemy;
    private readonly EnemyStats _enemyStats;

    public EnemyIdleState(
        Enemy enemy,
        AnimatorController animatorController,
        Ball ball,
        Mover mover,
        CollisionHandler collisionHandler,
        Collider squadZone,
        Collider collider,
        Rigidbody rigidbody,
        EnemyStats enemyStats)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody)
    {
        _enemy = enemy;
        _enemyStats = enemyStats;
    }

    protected override float IdleMinStandTime => _enemyStats.IdleMinStandTime;
    protected override float IdleMaxStandTime => _enemyStats.IdleMaxStandTime;
    protected override float WalkSpeed => _enemyStats.WalkSpeed;
    protected override float RotationSpeed => _enemyStats.RotationSpeed;

    protected override Transform GetTransform() => _enemy.transform;

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