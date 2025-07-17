using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
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

    private CancellationTokenSource _cancellationTokenSource;

    protected readonly List<Entity> Teammates;
    protected readonly Collider SquadZone;
    protected IStateSwitcher StateSwitcher;

    protected EntityIdleState(
        AnimatorController animatorController, Ball ball, Mover mover,
        CollisionHandler collisionHandler, Collider squadZone,
        Collider collider, Rigidbody rigidbody, Entity entity, EntityConfig entityConfig, List<Entity> teammates)
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
        Teammates = teammates;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public virtual void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        GameStatusService.Instance.OnHolderChanged += HandleHolderChanged;

        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;

        _animatorController.Idle();
        RunIdleMovementLoop(_cancellationTokenSource.Token).Forget();

        HandleHolderChanged(GameStatusService.Instance.CurrentHolder);

        FindTarget(_cancellationTokenSource.Token).Forget();
        TryMoveToBall(_cancellationTokenSource.Token).Forget();
    }

    public virtual void Exit()
    {
        _cancellationTokenSource.Cancel();

        GameStatusService.Instance.OnHolderChanged -= HandleHolderChanged;

        _rigidbody.isKinematic = false;
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, _entity.transform);
    }

    private async UniTaskVoid RunIdleMovementLoop(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            float standTime = Random.Range(_entityConfig.IdleMinStandTime, _entityConfig.IdleMaxStandTime);
            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);

            _animatorController.DodgeIdle();
            await _mover.MoveTo(target, _entityConfig.WalkSpeed, token);
            
            _animatorController.Idle();
            await UniTask.Delay((int)(standTime * 1000), cancellationToken: token);
        }
    }

    private async UniTaskVoid FindTarget(CancellationToken token)
    {
        float delay = 1f;

        while (token.IsCancellationRequested == false)
        {
            HandleHolderChanged(GameStatusService.Instance.CurrentHolder);
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
        }
    }

    private async UniTaskVoid TryMoveToBall(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            await UniTask.WaitForFixedUpdate(cancellationToken: token);

            if (GameStatusService.Instance.CurrentZone != SquadZone)
                continue;

            if (GameStatusService.Instance.CurrentHolder != null)
                continue;
            
            if (GameStatusService.Instance.CurrentBall.Chargeable.IsCharged)
                continue;

            SwitchToMove();
        }
    }

    protected abstract void SwitchToMove();

    protected abstract void HandleHolderChanged(Entity entity);
}