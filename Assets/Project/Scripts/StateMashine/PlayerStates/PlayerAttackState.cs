using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackState : IState
{
    private readonly Player _player;
    private readonly BallHolder _ballHolder;
    private readonly TargetScanner _targetScanner;
    private readonly TargetProvider _targetProvider;
    private readonly List<Entity> _teammates;
    private readonly PlayerInputController _inputController;
    private readonly BallThrower _ballThrower;

    private IStateSwitcher _stateSwitcher;

    public PlayerAttackState(Player player, BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider,
        List<Entity> teammates, PlayerInputController inputController, BallThrower ballThrower)
    {
        _player = player;
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

        _inputController.ActionButtonStarted += OnButtonClicked;
        _inputController.ActionButtonCanceled += OnButtonReleased;
    }

    public void Exit()
    {
        _inputController.ActionButtonStarted -= OnButtonClicked;
        _inputController.ActionButtonCanceled -= OnButtonReleased;
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
    }

    private void OnButtonReleased()
    {
        Ball ball = _ballHolder.LostBall();
        _ballThrower.StopCharging();
        _ballThrower.Throw(ball);

        _stateSwitcher.SwitchState<PlayerIdleState>();
    }
}