public class PlayerMoveState : IState
{
    private IStateSwitcher _stateSwitcher;
    private Player _player;

    public PlayerMoveState(Player player)
    {
        _player = player;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _player.PlayerInputController.ActionButtonStarted += Jump;
    }

    public virtual void Exit()
    {
        _player.PlayerInputController.ActionButtonStarted -= Jump;
    }

    public virtual void Update()
    {
        if (_player.GroundChecker.IsGrounded && _player.TargetProvider.Ball != null)
        {
            _player.Mover.Move(_player.TargetProvider.Ball.transform.position, _player.PlayerStats.Speed,
                _player.PlayerStats.RotationSpeed);
        }
        else if (_player.TargetProvider.Ball == null)
        {
            _stateSwitcher.SwitchState<PlayerIdleState>();
        }

        if (_player.BallHolder.HasBall)
            _stateSwitcher.SwitchState<PlayerAttackState>();
    }

    private void Jump()
    {
        _stateSwitcher.SwitchState<PlayerJumpState>();
    }
}