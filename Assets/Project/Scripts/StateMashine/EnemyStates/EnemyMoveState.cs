using UniRx;
using UnityEngine;

public class EnemyMoveState : IState
{
    private readonly Enemy _enemy;
    private readonly EnemyStats _enemyEnemyStats;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly CompositeDisposable _disposable;
    private readonly BallHolder _ballHolder;
    private readonly Ball _ball;

    private IStateSwitcher _stateSwitcher;

    private Coroutine _moveRoutine;

    public EnemyMoveState(Enemy enemy, EnemyStats enemyStats, CollisionHandler collisionHandler,
        Collider squadZone, CompositeDisposable compositeDisposable, BallHolder ballHolder, Ball ball)
    {
        _enemy = enemy;
        _enemyEnemyStats = enemyStats;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _disposable = compositeDisposable;
        _ballHolder = ballHolder;
        _ball = ball;

        _collisionHandler.BallDetected += OnBallDetected;

        MessageBrokerHolder.GameActions.Receive<M_BallTaken>()
            .Subscribe(message => HandleBallPositionChanged(message.Position))
            .AddTo(_disposable);

        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        Vector3 direction = (_ball.transform.position - _enemy.transform.position);
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _enemy.transform.rotation = Quaternion.RotateTowards(
                _enemy.transform.rotation,
                targetRotation,
                _enemyEnemyStats.RotationSpeed * Time.deltaTime
            );
        }

        _enemy.transform.position = Vector3.MoveTowards(
            _enemy.transform.position,
            _ball.transform.position,
            _enemyEnemyStats.RunSpeed * Time.deltaTime
        );
    }

    private void OnBallDetected(Ball ball)
    {
        _ballHolder.EquipBall(ball);
        _stateSwitcher.SwitchState<EnemyAttackState>();
    }

    private void HandleBallZoneChanged(Collider zone)
    {
        if (zone == _squadZone)
        {
            _stateSwitcher.SwitchState<EnemyMoveState>();
        }
        else
        {
            _stateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }

    private void HandleBallPositionChanged(Vector3 ballPosition)
    {
        Vector3 closestPoint = _squadZone.ClosestPoint(ballPosition);

        if (closestPoint == ballPosition)
        {
            _stateSwitcher.SwitchState<EnemyIdleState>();
        }
        else
        {
            _stateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }
}