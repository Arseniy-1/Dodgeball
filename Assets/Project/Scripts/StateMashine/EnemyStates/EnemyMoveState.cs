using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class EnemyMoveState : IState
{
    private readonly Enemy _enemy;
    private readonly AnimatorController _animatorController;
    private readonly List<Entity> _teammates;
    private readonly EnemyStats _enemyEnemyStats;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly BallHolder _ballHolder;
    private readonly Ball _ball;
    private readonly Collider _collider;
    private CompositeDisposable _disposable;

    private IStateSwitcher _stateSwitcher;

    private Coroutine _moveRoutine;

    public EnemyMoveState(Enemy enemy,AnimatorController animatorController, List<Entity> teammates, EnemyStats enemyStats, CollisionHandler collisionHandler,
        Collider squadZone, BallHolder ballHolder, Ball ball, Collider collider)
    {
        _enemy = enemy;
        _animatorController = animatorController;
        _teammates = teammates;
        _enemyEnemyStats = enemyStats;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _ballHolder = ballHolder;
        _ball = ball;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
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

    public void Exit()
    {
        _collisionHandler.BallDetected -= OnBallDetected;
        _disposable.Dispose();
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
        if (zone != _squadZone)
        {
            _stateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }

    private void HandleBallTaken(Entity entity)
    {
        if (_teammates.Contains(entity) == false)
        {
            _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
        else
        {
            if (entity != _enemy)
            {
                _stateSwitcher.SwitchState<PlayerIdleState>();
            }
        }
    }
}