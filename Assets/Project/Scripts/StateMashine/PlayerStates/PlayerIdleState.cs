using UnityEngine;

public class PlayerIdleState : EntityIdleState
{
    private readonly Player _player;
    private readonly PlayerStats _playerStats;

    public PlayerIdleState(
        Player player,
        AnimatorController animatorController,
        Ball ball,
        Mover mover,
        CollisionHandler collisionHandler,
        Collider squadZone,
        Collider collider,
        Rigidbody rigidbody,
        PlayerStats playerStats)
        : base(animatorController, ball, mover, collisionHandler, squadZone, collider, rigidbody)
    {
        _player = player;
        _playerStats = playerStats;
    }

    protected override float IdleMinStandTime => _playerStats.IdleMinStandTime;
    protected override float IdleMaxStandTime => _playerStats.IdleMaxStandTime;
    protected override float WalkSpeed => _playerStats.WalkSpeed;
    protected override float RotationSpeed => _playerStats.RotationSpeed;

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