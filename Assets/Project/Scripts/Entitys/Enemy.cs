using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class Enemy : Entity, IDestoyable<Enemy>
{
    [SerializeField] private EnemyStats _enemyStats;
    
    public event Action<Enemy> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teamates, Ball ball)
    {
        base.Initialize(squadZone, teamates, ball);
        BallThrower.Initialize(_enemyStats);
        
        List<IState> enemyStates = new List<IState>
        {
            new EnemyIdleState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyStats,
                CompositeDisposable),
            new EnemyMoveState(this, _enemyStats, CollisionHandler, SquadZone, CompositeDisposable, BallHolder,
                ball),
            new EnemyDodgeState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyStats,
                CompositeDisposable),
            new EnemyAttackState(this, BallHolder, TargetScanner, TargetProvider, Teamates, BallThrower, _enemyStats),
            new EnemyJumpState(_enemyStats, Rigidbody, GroundChecker, CollisionHandler, Collider)
        };
        StateMashine = new StateMashine(enemyStates);

        foreach (var state in enemyStates)
            state.Initialize(StateMashine);
    }
    
    public override void Reset()
    {
        Health.Reset();
        StateMashine.SwitchState<EnemyIdleState>();
    }

    [Button]
    protected override void Die()
    {
        OnDestroyed?.Invoke(this);
    }
}