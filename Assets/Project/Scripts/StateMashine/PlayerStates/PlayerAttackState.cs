using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerAttackState : IState
{
    private readonly Player _player;
    private readonly AnimatorController _animatorController;
    private readonly BallHolder _ballHolder;
    private readonly TargetScanner _targetScanner;
    private readonly TargetProvider _targetProvider;
    private readonly List<Entity> _teammates;
    private readonly PlayerInputController _inputController;
    private readonly BallThrower _ballThrower;

    private IStateSwitcher _stateSwitcher;

    private System.Action _buttonCanceledHandler;
    private System.Action _buttonStartedHandler;

    public PlayerAttackState(Player player, AnimatorController animatorController, BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider,
        List<Entity> teammates, PlayerInputController inputController, BallThrower ballThrower)
    {
        _player = player;
        _animatorController = animatorController;
        _ballHolder = ballHolder;
        _targetScanner = targetScanner;
        _targetProvider = targetProvider;
        _teammates = teammates;
        _inputController = inputController;
        _ballThrower = ballThrower;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        Entity target = _targetScanner.Scan(_teammates);
        _targetProvider.SelectTarget(target);

        _animatorController.Idle();

        _buttonStartedHandler = OnButtonClicked;
        _buttonCanceledHandler = () => _ = OnButtonReleasedAsync();

        _inputController.ActionButtonStarted += _buttonStartedHandler;
        _inputController.ActionButtonCanceled += _buttonCanceledHandler;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (_targetProvider.Target != null)
        {
            Vector3 direction = Vector3
                .ProjectOnPlane(_targetProvider.Target.transform.position - _player.transform.position,
                    Vector3.up).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;

            _player.transform.rotation = rotation;
        }
    }

    private void OnButtonClicked()
    {
        _ballThrower.StartCharging();
        _animatorController.PrepareAttack();
    }

    private async Task OnButtonReleasedAsync()
    {
        Ball ball = _ballHolder.LostBall();
        _ballThrower.StopCharging();
        _ballThrower.Throw(ball);

        _inputController.ActionButtonStarted -= _buttonStartedHandler;
        _inputController.ActionButtonCanceled -= _buttonCanceledHandler;
        
        await _animatorController.Attack();
        
        _stateSwitcher.SwitchState<PlayerIdleState>();
    }
}
