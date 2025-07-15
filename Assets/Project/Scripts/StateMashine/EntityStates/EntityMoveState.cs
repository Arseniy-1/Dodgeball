using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public abstract class EntityMoveState : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly Rotator _rotator;
    private readonly EntityConfig _entityConfig;
    private readonly Mover _mover;

    private CancellationTokenSource _cancellationTokenSource;
    private Ball _targetBall;

    protected readonly BallHolder BallHolder;
    protected readonly Collider SquadZone;
    protected IStateSwitcher StateSwitcher;

    protected EntityMoveState(Entity entity, AnimatorController animatorController, CollisionHandler collisionHandler,
        Collider squadZone, BallHolder ballHolder, Collider collider, EntityConfig entityConfig, Mover mover)
    {
        _entity = entity;
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        BallHolder = ballHolder;
        _collider = collider;
        _entityConfig = entityConfig;
        _mover = mover;
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _targetBall = GameStatusService.Instance.CurrentBall;

        _collisionHandler.BallDetected += OnBallDetected;
        GameStatusService.Instance.OnZoneChanged += HandleBallZoneChanged;
        GameStatusService.Instance.OnHolderChanged += HandleBallTaken;

        _collisionHandler.enabled = true;
        _collider.enabled = true;
        _collider.isTrigger = false;

        _animatorController.Run();
    }

    public virtual void Exit()
    {
        _collisionHandler.BallDetected -= OnBallDetected;
        GameStatusService.Instance.OnZoneChanged -= HandleBallZoneChanged;
        GameStatusService.Instance.OnHolderChanged -= HandleBallTaken;
        
        _cancellationTokenSource.Cancel();
        _targetBall = null;
    }

    public virtual void Update()
    {
        if (_targetBall == null)
            return;

        _rotator.RotateToTarget(_targetBall.transform, _entity.transform, _entityConfig.RotationSpeed);
        _mover.FollowTarget(_targetBall.transform, _entityConfig.RunSpeed);
    }

    protected virtual void OnBallDetected(Ball ball)
    {
        GameStatusService.Instance.SetHolder(_entity);
    }

    protected abstract void HandleBallZoneChanged(Collider zone);
    protected abstract void HandleBallTaken(Entity entity);
}