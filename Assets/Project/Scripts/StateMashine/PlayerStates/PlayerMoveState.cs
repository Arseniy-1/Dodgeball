using UniRx;
using UnityEngine;
using System.Collections;

public class PlayerMoveState : IState
{
    private readonly Player _player;
    private readonly PlayerStats _playerStats;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly CompositeDisposable _disposable;
    private readonly BallHolder _ballHolder;
    private readonly Ball _ball;
    private readonly Collider _collider;

    private IStateSwitcher _stateSwitcher;
    
    private Coroutine _moveRoutine;

    public PlayerMoveState(Player player, PlayerStats stats, Mover mover, CollisionHandler collisionHandler,
        Collider squadZone,
        CompositeDisposable compositeDisposable, BallHolder ballHolder, Ball ball, Collider collider)
    {
        _player = player;
        _playerStats = stats;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _disposable = compositeDisposable;
        _ballHolder = ballHolder;
        _ball = ball;
        _collider = collider;

        _collisionHandler.BallDetected += OnBallDetected;

        MessageBrokerHolder.GameActions.Receive<M_BallTaken>().Subscribe(message => HandleBallPositionChanged(message.Position))
            .AddTo(_disposable);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>().Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }

    public void Exit()
    { 
    }

    public void Update()
    {
        Vector3 direction = (_ball.transform.position - _player.transform.position);
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _player.transform.rotation = Quaternion.RotateTowards(
                _player.transform.rotation,
                targetRotation,
                _playerStats.RotationSpeed * Time.deltaTime
            );
        }
        
        _player.transform.position = Vector3.MoveTowards(
            _player.transform.position,
            _ball.transform.position,
            _playerStats.RunSpeed * Time.deltaTime
        );
    }
    
    private void OnBallDetected(Ball ball)
    {
        _ballHolder.EquipBall(ball);
        _stateSwitcher.SwitchState<PlayerAttackState>();
    }

    private void HandleBallZoneChanged(Collider zone)
    {
        if (zone == _squadZone)
        {
            _stateSwitcher.SwitchState<PlayerMoveState>();
        }
        else
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }

    private void HandleBallPositionChanged(Vector3 ballPosition)
    {
        Debug.Log(_squadZone==null);
        
        Vector3 closestPoint = _squadZone.ClosestPoint(ballPosition);

        if (closestPoint == ballPosition)
        {
            _stateSwitcher.SwitchState<PlayerIdleState>();
        }
        else
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}