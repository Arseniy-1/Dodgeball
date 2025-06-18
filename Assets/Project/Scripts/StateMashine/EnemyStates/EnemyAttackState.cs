using UnityEngine;
using System.Collections.Generic;

public class EnemyAttackState : EntityAttackState
{
    private readonly EnemyStats _enemyStats;

    private float _shootDelay;
    private float _releaseTimer;
    private bool _hasReleased;

    public EnemyAttackState(Enemy enemy, CollisionHandler collisionHandler,
        Collider collider, Rigidbody rigidbody,
        AnimatorController animatorController,
        BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider, List<Entity> teammates,
        BallThrower ballThrower, EnemyStats enemyStats) :
        base(enemy, collisionHandler, collider, rigidbody,
            animatorController, ballHolder, targetScanner,
            targetProvider, teammates, ballThrower)
    {
        _enemyStats = enemyStats;
    }


    public override void Enter()
    {
        base.Enter();

        if (TargetProvider.Target != null)
            StartAttack();

        _shootDelay = Random.Range(_enemyStats.MinThrowWait, _enemyStats.MaxThrowWait);
        _releaseTimer = 0f;
        _hasReleased = false;
    }

    public override async void Update()
    {
        base.Update();

        if (_hasReleased == false)
        {
            _releaseTimer += Time.deltaTime;

            if (_releaseTimer >= _shootDelay)
            {
                await ThrowBall();
                StateSwitcher.SwitchState<EnemyIdleState>();
                _hasReleased = true;
            }
        }
    }
}