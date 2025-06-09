using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class EnemyPrepareState : IState
{
    private readonly Enemy _enemy;
    private readonly AnimatorController _animatorController;
    private readonly Collider _squadZone;
    private CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;

    private IDisposable _movementLoopDisposable;

    public EnemyPrepareState(Enemy enemy, AnimatorController animatorController, Collider squadZone)
    {
        _enemy = enemy;
        _animatorController = animatorController;
        _squadZone = squadZone;
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
        LookRandom();
    }

    public void Exit()
    {
        _disposable.Dispose();
    }

    public void Update()
    {
    }

    private void LookRandom()
    {
        Vector3 randomDirection = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            0f,
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized;

        if (randomDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
            _enemy.transform.rotation = targetRotation;
        }
    }



    private void HandleStartGame()
    {
        _stateSwitcher.SwitchState<EnemyIdleState>();
    }
}