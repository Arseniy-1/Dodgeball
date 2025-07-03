using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EntityIdleState
{
    public EnemyIdleState(Enemy enemy,
        AnimatorController animatorController, Ball ball, Mover mover, CollisionHandler collisionHandler,
        Collider squadZone, Collider collider, Rigidbody rigidbody, EnemyConfig enemyConfig, List<Entity> teammates)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, enemy, enemyConfig, teammates)
    {
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
        {
            if (BallService.Instance.CurrentHolder != null)
                return;
            
            StateSwitcher.SwitchState<EnemyMoveState>();
        }
    }
    
    protected override void HandleBallTaken(Entity entity)
    {
        if (Teammates.Contains(entity) == false)
        {
            StateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }
}