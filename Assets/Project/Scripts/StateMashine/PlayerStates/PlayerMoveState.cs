using UniRx;
using UnityEngine;
using System.Collections;

public class PlayerMoveState : IState
{
    private readonly Player _player;
    private readonly PlayerStats _playerStats;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly BallHolder _ballHolder;
    private readonly Ball _ball;
    private readonly Collider _collider;
    private readonly CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;
    
    private Coroutine _moveRoutine;

    public PlayerMoveState(Player player, PlayerStats stats, CollisionHandler collisionHandler, Collider squadZone, BallHolder ballHolder, Ball ball, Collider collider)
    {
        _player = player;
        _playerStats = stats;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _ballHolder = ballHolder;
        _ball = ball;
        _collider = collider;
        _disposable = new CompositeDisposable();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _collisionHandler.BallDetected += OnBallDetected;
        
        MessageBrokerHolder.GameActions.Receive<M_BallTaken>().Subscribe(message => HandleBallPositionChanged(message.Entity))
            .AddTo(_disposable);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>().Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
        
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }

    public void Exit()
    { 
        _collisionHandler.BallDetected -= OnBallDetected;
        _disposable.Dispose();
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
        if (zone != _squadZone)
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }

    private void HandleBallPositionChanged(Entity entity)
    {
        if(entity == _player)
            return;
        
        Vector3 closestPoint = _squadZone.ClosestPoint(entity.transform.position);

        if (closestPoint == entity.transform.position)
        {
            _stateSwitcher.SwitchState<PlayerIdleState>();
        }
        else
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}