using UniRx;
using UnityEngine;

public class PlayerMoveState : IState
{
    private IStateSwitcher _stateSwitcher;
    private Player _player;
    private Collider _squadZone;
    private CompositeDisposable _disposable;
    private CollisionHandler _collisionHandler;
    private BallHolder _ballHolder;

    public PlayerMoveState(Player player, CollisionHandler collisionHandler, Collider squadZone,
        CompositeDisposable compositeDisposable, BallHolder ballHolder)
    {
        _player = player;
        _squadZone = squadZone;
        _disposable = compositeDisposable;
        _collisionHandler = collisionHandler;
        _ballHolder = ballHolder;

        _collisionHandler.BallDetected += OnBallDetected;

        MessageBrokerHolder.GameActions.Receive<M_BallTaken>().Subscribe((message) => HandleBallPositionChanged(message.Position))
            .AddTo(_disposable);
        
        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>().Subscribe((message) => HandleBallPositionChanged(message.Position))
            .AddTo(_disposable);
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }

    private void OnBallDetected(Ball ball)
    {
        _ballHolder.EquipBall(ball);
        _stateSwitcher.SwitchState<PlayerAttackState>();
        
    }

    private void HandleBallPositionChanged(Vector3 ballPosition)
    {
        if (_squadZone.bounds.Contains(ballPosition))
        {
            _stateSwitcher.SwitchState<PlayerIdleState>();
        }
        else
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}