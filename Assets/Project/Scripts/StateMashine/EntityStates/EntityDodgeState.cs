using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EntityDodgeState : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly Rigidbody _rigidbody;
    private readonly AreaPointSelector _areaPointSelector;
    private readonly Rotator _rotator;
    private readonly EntityConfig _entityConfig;

    protected readonly Collider SquadZone;

    private IDisposable _movementLoopDisposable;

    protected IStateSwitcher StateSwitcher;
    protected CompositeDisposable Disposable;

    protected EntityDodgeState(Entity entity, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, Rigidbody rigidbody, EntityConfig entityConfig)
    {
        _entity = entity;
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        SquadZone = squadZone;
        _rigidbody = rigidbody;
        _entityConfig = entityConfig;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher) => StateSwitcher = stateSwitcher;

    public virtual void Enter()
    {
        Disposable = new CompositeDisposable();
        _animatorController.DodgeIdle();

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(Disposable);

        _rigidbody.isKinematic = true;
        StartIdleMovementLoop();
    }

    public virtual void Exit()
    {
        Disposable.Dispose();
        _rigidbody.isKinematic = false;
        _mover.Stop();
        _movementLoopDisposable?.Dispose();
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityConfig.RotationSpeed);
    }

    protected abstract void HandleBallZoneChanged(Collider zone);

    private void StartIdleMovementLoop()
    {
        _movementLoopDisposable = Observable.FromCoroutine(IdleMovementLoop)
            .Subscribe()
            .AddTo(Disposable);
    }

    private System.Collections.IEnumerator IdleMovementLoop()
    {
        while (true)
        {
            float standTime = Random.Range(_entityConfig.DodgeDirectionChangeMinTime, _entityConfig.DodgeDirectionChangeMaxTime);
            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
            _animatorController.DodgeIdle();
            yield return _mover.MoveTo(target, _entityConfig.DodgeSpeed);
            _animatorController.Idle();
            yield return new WaitForSeconds(standTime);
        }
    }
}