using UnityEngine;

public class PlayerIdleState : EntityIdleState
{
    private readonly Player _player;

    public PlayerIdleState(Player player, AnimatorController animatorController, Ball ball,
        Mover mover, CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, PlayerStats playerStats)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody, playerStats)
    {
        _player = player;
    }

    protected override Transform GetTransform() => _player.transform;

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