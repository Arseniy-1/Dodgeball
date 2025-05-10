using UnityEngine;
using System.Collections.Generic;

public class EnemyAttackState : IState
{
    private readonly Enemy _enemy;
    private readonly BallHolder _ballHolder;
    private readonly TargetScanner _targetScanner;
    private readonly TargetProvider _targetProvider;
    private readonly List<Entity> _teammates;
    private readonly BallThrower _ballThrower;
    private readonly EnemyStats _enemyStats;
    
    private IStateSwitcher _stateSwitcher;

    private float _shootDelay;
    private float _releaseTimer;
    private bool _hasReleased;

    public EnemyAttackState(Enemy enemy, BallHolder ballHolder, TargetScanner targetScanner, TargetProvider targetProvider,
        List<Entity> teammates, BallThrower ballThrower, EnemyStats enemyStats)
    {
        _enemy = enemy;
        _ballHolder = ballHolder;
        _targetScanner = targetScanner;
        _targetProvider = targetProvider;
        _teammates = teammates;
        _ballThrower = ballThrower;
        _enemyStats = enemyStats;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        Entity target = _targetScanner.Scan(_teammates);
        _targetProvider.SelectTarget(target);

        StartAttack();

        _shootDelay = Random.Range(_enemyStats.MinThrowWait, _enemyStats.MaxThrowWait);
        _releaseTimer = 0f;
        _hasReleased = false;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (_targetProvider.Target != null)
        {
            Vector3 direction = Vector3
                .ProjectOnPlane(_targetProvider.Target.transform.position - _enemy.transform.position,
                    Vector3.up).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;

            _enemy.transform.rotation = rotation;
        }

        if (!_hasReleased)
        {
            _releaseTimer += Time.deltaTime;
            if (_releaseTimer >= _shootDelay)
            {
                ThrowBall();
                _hasReleased = true;
            }
        }
    }

    private void StartAttack()
    {
        _ballThrower.StartCharging();
    }

    private void ThrowBall()
    {
        Ball ball = _ballHolder.LostBall();
        _ballThrower.StopCharging();
        _ballThrower.Throw(ball);

        _stateSwitcher.SwitchState<EnemyIdleState>();
    }
}
