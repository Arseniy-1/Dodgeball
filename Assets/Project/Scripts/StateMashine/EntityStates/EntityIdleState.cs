using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EntityIdleState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    private readonly AreaPointSelector _areaPointSelector;
    private readonly Rotator _rotator;
    private readonly Entity _entity;
    private readonly EntityConfig _entityConfig;

    private CompositeDisposable _disposable;
    private IDisposable _movementLoopDisposable;

    protected readonly Collider SquadZone;

    protected IStateSwitcher StateSwitcher;
    
    protected EntityIdleState(
        AnimatorController animatorController, Ball ball, Mover mover,
        CollisionHandler collisionHandler, Collider squadZone, 
        Collider collider, Rigidbody rigidbody, Entity entity, EntityConfig entityConfig)
    {
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        _collider = collider;
        _rigidbody = rigidbody;
        _entity = entity;
        _entityConfig = entityConfig;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public virtual void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _disposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);

        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.enabled = false;

        _animatorController.Idle();
        StartIdleMovementLoop();
    }

    public virtual void Exit()
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
            float standTime = Random.Range(_entityConfig.IdleMinStandTime, _entityConfig.IdleMaxStandTime);

            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
            _animatorController.DodgeIdle();
            yield return _mover.MoveTo(target, _entityConfig.WalkSpeed);
            _animatorController.Idle();
            yield return new WaitForSeconds(standTime);
        }
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, _entity.transform);
    }

    protected abstract void HandleBallZoneChanged(Collider zone);
}
