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

    private CompositeDisposable _disposable;
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
        _disposable = new CompositeDisposable();

        GameStatusService.Instance.OnZoneChanged += HandleBallZoneChanged;
        GameStatusService.Instance.OnHolderChanged += HandleBallTaken;
        
        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;

        _animatorController.Idle();
        RunIdleMovementLoop(_cancellationTokenSource.Token).Forget();
 
        HandleBallTaken(GameStatusService.Instance.CurrentHolder);
        HandleBallZoneChanged(GameStatusService.Instance.CurrentZone);
        
        FindTarget(_cancellationTokenSource.Token).Forget();
    }

    public virtual void Exit()
    {
        _cancellationTokenSource?.Cancel();
        _disposable?.Dispose();

        GameStatusService.Instance.OnZoneChanged -= HandleBallZoneChanged;
        GameStatusService.Instance.OnHolderChanged -= HandleBallTaken;

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
        while (!token.IsCancellationRequested)
        {
            float standTime = Random.Range(_entityConfig.IdleMinStandTime, _entityConfig.IdleMaxStandTime);
            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);

            _animatorController.DodgeIdle();
            await _mover.MoveTo(target, _entityConfig.WalkSpeed, token);
            _animatorController.Idle();
            await UniTask.Delay((int)(standTime * 1000), cancellationToken: token);
        }
    }

    private async UniTask FindTarget(CancellationToken token)
    {
        float delay = 1f;
        
        while (token.IsCancellationRequested == false)
        {
            HandleBallTaken(GameStatusService.Instance.CurrentHolder);
            HandleBallZoneChanged(GameStatusService.Instance.CurrentZone);

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
        }
    }

    protected abstract void HandleBallZoneChanged(Collider zone);

    protected abstract void HandleBallTaken(Entity entity);
}