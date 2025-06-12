using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class EnemyPrepareState : IState
{
    private readonly Enemy _enemy;
    private readonly AnimatorController _animatorController;
    private readonly TargetScanner _targetScanner;
    private readonly TargetProvider _targetProvider;
    private readonly List<Entity> _teammates;
    private CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;

    private IDisposable _movementLoopDisposable;

    public EnemyPrepareState(Enemy enemy, AnimatorController animatorController, TargetScanner targetScanner,
        List<Entity> teammates)
    {
        _enemy = enemy;
        _animatorController = animatorController;
        _targetScanner = targetScanner;
        _teammates = teammates;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _disposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_GameStarted>()
            .Subscribe(message => HandleStartGame())
            .AddTo(_disposable);

        _animatorController.PrepareToBattle();
        LookToTarget();
    }

    public void Exit()
    {
        _disposable.Dispose();
    }

    public void Update()
    {
    }

    private void LookToTarget()
    {
        Entity target = _targetScanner.Scan(_teammates);

        if (target == null)
            return;

        Vector3 direction = target.transform.position - _enemy.transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _enemy.transform.rotation = targetRotation;
        }
    }

    private void HandleStartGame()
    {
        _stateSwitcher.SwitchState<EnemyIdleState>();
    }
}