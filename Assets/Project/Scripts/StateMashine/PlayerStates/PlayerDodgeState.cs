using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDodgeState : IState
{
    private readonly Player _player;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly PlayerInputController _playerInputController;
    private readonly Collider _squadZone;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    private readonly PlayerStats _playerStats;
    private readonly AreaPointSelector _areaPointSelector;
    private CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;

    private IDisposable _movementLoopDisposable;

    public PlayerDodgeState(Player player, Ball ball, Mover mover, CollisionHandler collisionHandler,
        Collider squadZone,
        Collider collider, Rigidbody rigidbody, PlayerStats playerStats, PlayerInputController playerInputController)
    {
        _player = player;
        _ball = ball;
        _mover = mover;
        _playerInputController = playerInputController;
        _squadZone = squadZone;
        _collisionHandler = collisionHandler;
        _collider = collider;
        _rigidbody = rigidbody;
        _playerStats = playerStats;
        _areaPointSelector = new AreaPointSelector();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _disposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);

        _rigidbody.isKinematic = true;

        _playerInputController.ActionButtonStarted += Jump;
        StartIdleMovementLoop();
        
        Debug.Log("&&&");
    }

    public void Exit()
    {
        _disposable.Dispose();

        _rigidbody.isKinematic = false;

        _playerInputController.ActionButtonStarted -= Jump;
        _mover.Stop();
        _movementLoopDisposable?.Dispose();
        
        Debug.Log("###");
    }

    private void StartIdleMovementLoop()
    {
        _movementLoopDisposable = Observable.FromCoroutine(IdleMovementLoop)
            .Subscribe()
            .AddTo(_disposable);
    }

    private System.Collections.IEnumerator IdleMovementLoop()
    {
        float standTime = Random.Range(_playerStats.DodgeDirectionChangeMinTime,
            _playerStats.DodgeDirectionChangeMaxTime);

        Vector3 target = _areaPointSelector.GetRandomPointInZone(_squadZone, _player.transform.position);

        yield return _mover.MoveTo(target, _playerStats.DodgeSpeed);
        yield return new WaitForSeconds(standTime);
    }

    public void Update()
    {
        Vector3 direction = (_ball.transform.position - _player.transform.position);
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _player.transform.rotation = Quaternion.RotateTowards(
                _player.transform.rotation,
                targetRotation,
                _playerStats.RotationSpeed * Time.deltaTime
            );
        }
    }

    private void HandleBallZoneChanged(Collider zone)
    {
        if (zone == _squadZone)
        {
            _stateSwitcher.SwitchState<PlayerMoveState>();
        }
    }

    private void Jump()
    {
        _stateSwitcher.SwitchState<PlayerJumpState>();
    }
}