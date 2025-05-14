using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerIdleState : IState
{
    private readonly Player _player;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    private readonly PlayerStats _playerStats;
    private readonly CompositeDisposable _disposable;
    
    private IStateSwitcher _stateSwitcher;
    
    private IDisposable _movementLoopDisposable;

    public PlayerIdleState(Player player, Ball ball, Mover mover, CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, PlayerStats playerStats, CompositeDisposable disposable)
    {
        _player = player;
        _ball = ball;
        _mover = mover;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _collider = collider;
        _rigidbody = rigidbody;
        _playerStats = playerStats;
        _disposable = new CompositeDisposable();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        Debug.Log("Enter");
        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
        
        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.enabled = false;

        StartIdleMovementLoop();
    }

    public void Exit()
    {
        _disposable.Dispose();
        
        _rigidbody.isKinematic = false;
        _collisionHandler.enabled = true;
        _collider.enabled = true;

        _mover.Stop();
        _movementLoopDisposable?.Dispose();
    }

    private void StartIdleMovementLoop()
    {
        _movementLoopDisposable = Observable.FromCoroutine(IdleMovementLoop)
            .Subscribe()
            .AddTo(_disposable);
    }

    private System.Collections.IEnumerator IdleMovementLoop()
    {
        while (true)
        {
            float standTime = Random.Range(_playerStats.IdleMinStandTime, _playerStats.IdleMaxStandTime);
            
            Vector3 target = GetRandomPointInZone();
            yield return _mover.MoveTo(target, _playerStats.WalkSpeed);
            yield return new WaitForSeconds(standTime);
        }
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
        Debug.Log("HandleBallZoneChanged");
        if (zone == _squadZone)
        {
            _stateSwitcher.SwitchState<PlayerMoveState>();
        }
        else
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }

    private Vector3 GetRandomPointInZone()
    {
        Bounds bounds = _squadZone.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = _player.transform.position.y;

        return new Vector3(x, y, z);
    }
}