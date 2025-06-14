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

    private CompositeDisposable _disposable;
    private IDisposable _movementLoopDisposable;

    protected readonly Collider SquadZone;

    protected IStateSwitcher StateSwitcher;
    
    protected EntityIdleState(
        AnimatorController animatorController,
        Ball ball,
        Mover mover,
        CollisionHandler collisionHandler,
        Collider squadZone,
        Collider collider,
        Rigidbody rigidbody)
    {
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        _collider = collider;
        _rigidbody = rigidbody;
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

        StartIdleMovementLoop();
        _animatorController.DodgeIdle();
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

    protected abstract float IdleMinStandTime { get; }
    protected abstract float IdleMaxStandTime { get; }
    protected abstract float WalkSpeed { get; }
    protected abstract float RotationSpeed { get; }

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
            float standTime = Random.Range(IdleMinStandTime, IdleMaxStandTime);

            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, GetTransform().position);
            _animatorController.DodgeIdle();
            yield return _mover.MoveTo(target, WalkSpeed);
            _animatorController.Idle();
            yield return new WaitForSeconds(standTime);
        }
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, GetTransform().transform);
    }

    protected abstract Transform GetTransform();

    protected abstract void HandleBallZoneChanged(Collider zone);
}
