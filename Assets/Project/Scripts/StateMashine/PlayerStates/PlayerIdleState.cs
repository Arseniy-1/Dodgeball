using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : EntityIdleState
{
    public PlayerIdleState(Player player, AnimatorController animatorController, Ball ball,
        Mover mover, CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, PlayerConfig playerConfig, List<Entity> teammates)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, player, playerConfig,
            teammates)
    {
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
        {
            if (GameStatusService.Instance.CurrentHolder != null)
                return;

            StateSwitcher.SwitchState<PlayerMoveState>();
        }
    }

    protected override void HandleBallTaken(Entity entity)
    {
        if(entity == null)
            return;
        
        if (Teammates.Contains(entity) == false)
        {
            StateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}