using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class EnemyAttackState : EntityAttackState
{
    private readonly EnemyConfig _enemyConfig;

    private CancellationTokenSource _attackCancelationTokenSource;

    public EnemyAttackState(Enemy enemy, CollisionHandler collisionHandler, Collider collider, Rigidbody rigidbody,
        AnimatorController animatorController, BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider, List<Entity> teammates, BallThrower ballThrower, EnemyConfig enemyConfig) :
        base(enemy, collisionHandler, collider, rigidbody, animatorController, 
            ballHolder, targetScanner, targetProvider, teammates, ballThrower)
    {
        _enemyConfig = enemyConfig;
    }

    public override async void Enter()
    {
        _attackCancelationTokenSource = new CancellationTokenSource();

        base.Enter();
        await ApplyTarget();

        Attack(_attackCancelationTokenSource.Token);
    }

    public override void Exit()
    {
        _attackCancelationTokenSource.Cancel();
        base.Exit();
    }

    private async void Attack(CancellationToken token)
    {
        StartAttack();

        float shootDelay = Random.Range(_enemyConfig.MinThrowWait, _enemyConfig.MaxThrowWait);

        await UniTask.Delay(TimeSpan.FromSeconds(shootDelay), cancellationToken: token);
        await ThrowBall();
        
        StateSwitcher.SwitchState<EnemyIdleState>();
    }
}