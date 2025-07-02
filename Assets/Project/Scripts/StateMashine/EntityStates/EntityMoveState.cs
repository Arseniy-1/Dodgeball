using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public abstract class EntityMoveState : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly Ball _ball;
    private readonly Collider _collider;
    private readonly Rotator _rotator;
    private readonly EntityConfig _entityConfig;
    private readonly Mover _mover;

    private CancellationTokenSource _cancellationTokenSource;

    protected readonly BallHolder BallHolder;
    protected readonly Collider SquadZone;

    protected IStateSwitcher StateSwitcher;

    protected EntityMoveState(Entity entity, AnimatorController animatorController, CollisionHandler collisionHandler,
        Collider squadZone, BallHolder ballHolder, Ball ball, Collider collider, EntityConfig entityConfig, Mover mover)
    {
        _entity = entity;
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        BallHolder = ballHolder;
        _ball = ball;
        _collider = collider;
        _entityConfig = entityConfig;
        _rotator = new Rotator();
        _mover = mover;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _collisionHandler.BallDetected += OnBallDetected;

        MessageBrokerHolder.GameActions.Receive<M_BallTaken>()
            .Subscribe(message => HandleBallTaken(message.Entity))
            .AddTo(_cancellationTokenSource.Token);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_cancellationTokenSource.Token);

        _collisionHandler.enabled = true;
        _collider.enabled = true;

        _animatorController.Run();
    }

    public virtual void Exit()
    {
        _collisionHandler.BallDetected -= OnBallDetected;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    public virtual void Update()
    {
        if (_ball == null)
            return;

        _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityConfig.RotationSpeed);
        _mover.FollowTarget(_ball.transform, _entityConfig.RunSpeed);
    }

    protected abstract void OnBallDetected(Ball ball);
    protected abstract void HandleBallZoneChanged(Collider zone);
    protected abstract void HandleBallTaken(Entity entity);
}