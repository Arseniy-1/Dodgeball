using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackState : IState
{
    private readonly Player _player;
    private BallHolder _ballHolder;
    private TargetScanner _targetScanner;
    private TargetProvider _targetProvider;
    private List<Entity> _teamates;
    private PlayerInputController _inputController;
    private BallThrower _ballThrower;

    private IStateSwitcher _stateSwitcher;

    public PlayerAttackState(Player player, BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider,
        List<Entity> teamates, PlayerInputController inputController, BallThrower ballThrower)
    {
        _player = player;
        _ballHolder = ballHolder;
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
            Debug.Log("2d");
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