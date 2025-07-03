using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EntityMoveState
{
    private readonly Enemy _enemy;   
    private readonly List<Entity> _teammates;

    public EnemyMoveState(Enemy enemy, AnimatorController animatorController,List<Entity> teammates, EnemyConfig enemyConfig,
        CollisionHandler collisionHandler, Collider squadZone,
        BallHolder ballHolder, Ball ball, Collider collider, Mover mover)
        : base(enemy, animatorController, collisionHandler, squadZone, ballHolder, collider, enemyConfig, mover)
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
        if (_teammates.Contains(entity) == false)
        {
            StateSwitcher.SwitchState<EnemyDodgeState>();
        }
        else if (entity != _enemy)
        {
            StateSwitcher.SwitchState<EnemyIdleState>();
        }
    }
}