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
    private readonly EntityStats _entityStats;
    private readonly Mover _mover;

    private CompositeDisposable _disposable;

    protected readonly BallHolder BallHolder;
    protected readonly Collider SquadZone;

    protected IStateSwitcher StateSwitcher;

    protected EntityMoveState(Entity entity, AnimatorController animatorController, CollisionHandler collisionHandler,
        Collider squadZone, BallHolder ballHolder, Ball ball, Collider collider, EntityStats entityStats, Mover mover)
    {
        _entity = entity;
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        BallHolder = ballHolder;
        _ball = ball;
        _collider = collider;
        _entityStats = entityStats;
        _rotator = new Rotator();
        _mover = mover;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _disposable = new CompositeDisposable();

        _collisionHandler.BallDetected += OnBallDetected;

        MessageBrokerHolder.GameActions.Receive<M_BallTaken>()
            .Subscribe(message => HandleBallTaken(message.Entity))
            .AddTo(_disposable);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);

        _collisionHandler.enabled = true;
        _collider.enabled = true;

        _animatorController.Run();
    }

    public virtual void Exit()
    {
        _collisionHandler.BallDetected -= OnBallDetected;
        _disposable.Dispose();
        _mover.Stop();
    }

    public virtual void Update()
    {
        if (_ball == null)
            return;

        _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityStats.RotationSpeed);
        _mover.FollowTarget(_ball.transform, _entityStats.RunSpeed);
    }

    protected abstract void OnBallDetected(Ball ball);
    protected abstract void HandleBallZoneChanged(Collider zone);
    protected abstract void HandleBallTaken(Entity entity);
}

