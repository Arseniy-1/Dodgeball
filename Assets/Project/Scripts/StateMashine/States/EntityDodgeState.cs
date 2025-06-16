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

    protected readonly Collider SquadZone;

    private IDisposable _movementLoopDisposable;

    protected IStateSwitcher StateSwitcher;
    protected CompositeDisposable Disposable;

    protected EntityDodgeState(Entity entity, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, Rigidbody rigidbody)
    {
        _entity = entity;
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        SquadZone = squadZone;
        _rigidbody = rigidbody;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher) => StateSwitcher = stateSwitcher;

    public virtual void Enter()
    {
        Disposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(Disposable);

        _rigidbody.isKinematic = true;
        StartIdleMovementLoop();
        _animatorController.DodgeIdle();
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
            _rotator.RotateToTarget(_ball.transform, _entity.transform, GetRotationSpeed());
    }

    protected abstract float GetRotationSpeed();
    protected abstract float GetMinDirectionChangeTime();
    protected abstract float GetMaxDirectionChangeTime();
    protected abstract float GetDodgeSpeed();

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
            float standTime = Random.Range(GetMinDirectionChangeTime(), GetMaxDirectionChangeTime());
            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
            _animatorController.DodgeIdle();
            yield return _mover.MoveTo(target, GetDodgeSpeed());
            _animatorController.Idle();
            yield return new WaitForSeconds(standTime);
        }
    }
}