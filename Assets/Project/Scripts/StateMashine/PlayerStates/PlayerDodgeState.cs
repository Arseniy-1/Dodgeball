using UnityEngine;

public class PlayerDodgeState : EntityDodgeState<Player>
{
    private readonly PlayerStats _playerStats;
    private readonly PlayerInputController _playerInputController;

    public PlayerDodgeState(Player player, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, 
        Rigidbody rigidbody, PlayerStats playerStats, PlayerInputController playerInputController)
        : base(player, animatorController, ball, mover, squadZone, rigidbody)
    {
        _playerStats = playerStats;
        _playerInputController = playerInputController;
    }

    public override void Enter()
    {
        base.Enter();
        _playerInputController.ActionButtonStarted += Jump;
    }

    public override void Exit()
    {
        base.Exit();
        _playerInputController.ActionButtonStarted -= Jump;
    }

    private void Jump() => StateSwitcher.SwitchState<PlayerJumpState>();

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
            StateSwitcher.SwitchState<PlayerMoveState>();
    }

    protected override float GetRotationSpeed() => _playerStats.RotationSpeed;
    protected override float GetMinDirectionChangeTime() => _playerStats.DodgeDirectionChangeMinTime;
    protected override float GetMaxDirectionChangeTime() => _playerStats.DodgeDirectionChangeMaxTime;
    protected override float GetDodgeSpeed() => _playerStats.DodgeSpeed;
}