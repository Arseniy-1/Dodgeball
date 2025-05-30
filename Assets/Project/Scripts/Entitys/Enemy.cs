using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class Enemy : Entity, IDestoyable<Enemy>
{
    [SerializeField] private EnemyStats _enemyStats;
    
    private List<IState> _enemyStates = new();
    
    public event Action<Enemy> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        base.Initialize(squadZone, teammates, ball);
        BallThrower.Initialize(_enemyStats);
        
        foreach (var state in _enemyStates)
        {
            if (state is IDisposable disposable)
                disposable.Dispose();
        }
        _enemyStates.Clear();
        
        _enemyStates = new List<IState>
        {
            new EnemyIdleState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyStats),
            new EnemyMoveState(this, _enemyStats, CollisionHandler, SquadZone, BallHolder, ball, Collider),
            new EnemyDodgeState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyStats),
            new EnemyAttackState(this, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, _enemyStats),
            new EnemyJumpState(_enemyStats, Rigidbody, GroundChecker, CollisionHandler, Collider)
        };
        
        // Пересоздание StateMashine
        StateMaсhine?.Dispose(); // если реализовано IDisposable — освободить ресурсы
        StateMaсhine = new StateMaсhine(_enemyStates);

        // Инициализация состояний
        foreach (var state in _enemyStates)
            state.Initialize(StateMaсhine);

        Reset();
    }
    
    public override void Reset()
    {
        base.Reset();
    }

    [Button]
    protected override void Die()
    {
        base.Die();
        OnDestroyed?.Invoke(this);
    }
}