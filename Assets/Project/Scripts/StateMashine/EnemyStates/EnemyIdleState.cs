using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyIdleState : IState
{
    private readonly Enemy _enemy;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    private readonly EnemyStats _enemyStats;
    private readonly CompositeDisposable _disposable;
    
    private IStateSwitcher _stateSwitcher;
    
    private IDisposable _movementLoopDisposable;

    public EnemyIdleState(Enemy enemy, Ball ball, Mover mover, CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, EnemyStats enemyStats, CompositeDisposable disposable)
    {
        _enemy = enemy;
        _ball = ball;
        _mover = mover;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _collider = collider;
        _rigidbody = rigidbody;
        _enemyStats = enemyStats;
        _disposable = disposable;

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.enabled = false;

        StartIdleMovementLoop();
    }

    public void Exit()
    {
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
            float standTime = Random.Range(_enemyStats.IdleMinStandTime, _enemyStats.IdleMaxStandTime);
            
            Vector3 target = GetRandomPointInZone();
            yield return _mover.MoveTo(target, _enemyStats.WalkSpeed);
            yield return new WaitForSeconds(standTime);
        }
    }
   
    public void Update()
    {            
        Vector3 direction = (_ball.transform.position - _enemy.transform.position);
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _enemy.transform.rotation = Quaternion.RotateTowards(
                _enemy.transform.rotation,
                targetRotation,
                _enemyStats.RotationSpeed * Time.deltaTime
            );
        }
    }
    
    private void HandleBallZoneChanged(Collider zone)
    {
        if (zone == _squadZone)
        {
            _stateSwitcher.SwitchState<EnemyMoveState>();
        }
        else
        {
            _stateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }

    private Vector3 GetRandomPointInZone()
    {
        Bounds bounds = _squadZone.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = _enemy.transform.position.y;

        return new Vector3(x, y, z);
    }
}