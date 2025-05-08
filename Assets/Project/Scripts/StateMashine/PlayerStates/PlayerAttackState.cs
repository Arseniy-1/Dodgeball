using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackState : IState
{
    private Player _player;
    private Ball _ball;
    private TargetScanner _targetScanner;
    private TargetProvider _targetProvider;
    private List<Entity> _teamates;
    private PlayerInputController _inputController;
    private BallThrower _ballThrower;

    private IStateSwitcher _stateSwitcher;

    public PlayerAttackState(Player player, TargetScanner targetScanner, TargetProvider targetProvider,
        List<Entity> teamates, PlayerInputController inputController, Collider collider, BallThrower ballThrower)
    {
        _player = player;
        _targetScanner = targetScanner;
        _targetProvider = targetProvider;
        _teamates = teamates;
        _inputController = inputController;
        _ballThrower = ballThrower;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        Entity target = _targetScanner.Scan(_teamates);
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
        if (_ball != null)
        {
            _ballThrower.StopCharging();
            _ball.transform.parent = null;
            _ballThrower.Throw(_ball);
        }

        _ball = null;

        _stateSwitcher.SwitchState<PlayerIdleState>();
    }
}