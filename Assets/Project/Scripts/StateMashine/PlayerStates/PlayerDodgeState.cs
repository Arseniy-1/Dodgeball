using UnityEngine;

public class PlayerDodgeState : EntityDodgeState
{
    private readonly PlayerInputController _playerInputController;
    
    public PlayerDodgeState(Player player, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, 
        Rigidbody rigidbody, PlayerConfig playerConfig, PlayerInputController playerInputController)
        : base(player, animatorController, ball, mover, squadZone, rigidbody, playerConfig)
    {
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

    private void Jump()
    {
        StateSwitcher.SwitchState<PlayerJumpState>();
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
        {
            StateSwitcher.SwitchState<PlayerMoveState>();
        }
    }
}