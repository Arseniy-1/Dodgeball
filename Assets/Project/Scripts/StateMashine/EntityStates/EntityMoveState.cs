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
    
    private CompositeDisposable _disposable;
    
    protected readonly BallHolder BallHolder;
    protected readonly Collider SquadZone;

    protected IStateSwitcher StateSwitcher;

    protected EntityMoveState(Entity entity, AnimatorController animatorController, CollisionHandler collisionHandler,
        Collider squadZone, BallHolder ballHolder, Ball ball, Collider collider, EntityStats entityStats)
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
            .Subscribe(message => 
                HandleBallTaken(message.Entity))
            .AddTo(_disposable);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>()
            .Subscribe(message => 
                HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);

        _collisionHandler.enabled = true;
        _collider.enabled = true;

        _animatorController.Run();
    }

    public virtual void Exit()
    {
        _collisionHandler.BallDetected -= OnBallDetected;
        _disposable.Dispose();
    }

    public virtual void Update()
    {
        if (_ball == null) 
            return;

        _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityStats.RotationSpeed);
        MoveToBall();
    }

    private void MoveToBall()
    {
        Vector3 currentPos = _entity.transform.position;
        Vector3 targetPos = _ball.transform.position;

        targetPos.y = currentPos.y;

        _entity.transform.position = Vector3.MoveTowards(
            currentPos,
            targetPos,
            _entityStats.RunSpeed * Time.deltaTime
        );
    }
    
    protected abstract void OnBallDetected(Ball ball);
    protected abstract void HandleBallZoneChanged(Collider zone);
    protected abstract void HandleBallTaken(Entity entity);
}
