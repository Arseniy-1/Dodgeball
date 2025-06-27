using UnityEngine;
using System.Collections.Generic;

public class EnemyAttackState : EntityAttackState
{
    private readonly EnemyConfig _enemyConfig;

    private float _shootDelay;
    private float _releaseTimer;
    private bool _hasReleased;

    public EnemyAttackState(Enemy enemy, CollisionHandler collisionHandler,
        Collider collider, Rigidbody rigidbody,
        AnimatorController animatorController,
        BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider, List<Entity> teammates,
        BallThrower ballThrower, EnemyConfig enemyConfig) :
        base(enemy, collisionHandler, collider, rigidbody,
            animatorController, ballHolder, targetScanner,
            targetProvider, teammates, ballThrower)
    {
        _enemyConfig = enemyConfig;
    }


    public override void Enter()
    {
        base.Enter();

        if (TargetProvider.Target != null)
            StartAttack();

        _shootDelay = Random.Range(_enemyConfig.MinThrowWait, _enemyConfig.MaxThrowWait);
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