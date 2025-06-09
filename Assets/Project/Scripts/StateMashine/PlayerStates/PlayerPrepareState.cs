using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class PlayerPrepareState : IState
{
    private readonly Player _player;
    private readonly AnimatorController _animatorController;
    private readonly Collider _squadZone;
    private CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;

    private IDisposable _movementLoopDisposable;

    public PlayerPrepareState(Player player, AnimatorController animatorController, Collider squadZone)
    {
        _player = player;
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
            _player.transform.rotation = targetRotation;
        }
    }

    private void HandleStartGame()
    {
        _stateSwitcher.SwitchState<PlayerIdleState>();
    }
}