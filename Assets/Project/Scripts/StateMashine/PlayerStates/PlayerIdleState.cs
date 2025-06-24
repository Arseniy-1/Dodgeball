using UnityEngine;

public class PlayerIdleState : EntityIdleState
{
    public PlayerIdleState(Player player, AnimatorController animatorController, Ball ball,
        Mover mover, CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, PlayerStats playerStats)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, player,playerStats)
    {
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
        {
            StateSwitcher.SwitchState<PlayerMoveState>();
        }
        else
        {
            StateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}