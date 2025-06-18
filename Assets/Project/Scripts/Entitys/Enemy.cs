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
            new EnemyPrepareState(this, AnimatorController, TargetScanner, Teammates),
            new EnemySelebrateState(this, AnimatorController, Teammates),
            new EnemyIdleState(this,AnimatorController, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _enemyStats),
            new EnemyMoveState(this, AnimatorController, _enemyStats, CollisionHandler, SquadZone, BallHolder, ball, Collider),
            new EnemyDodgeState(this, AnimatorController, ball, Mover, SquadZone, Rigidbody, _enemyStats),
            new EnemyAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, _enemyStats),
            new EnemyJumpState(AnimatorController, CollisionHandler, HitCheker, Collider),
            new EnemyDeathState(AnimatorController, CollisionHandler, Collider, BallHolder)
        };
        
        StateMaсhine = new StateMaсhine(_enemyStates);

        foreach (var state in _enemyStates)
            state.Initialize(StateMaсhine);

        Reset();
    }
    
    [Button]
    protected override async void HandleLostHealth()
    {
        StateMaсhine.SwitchState<EnemyDeathState>();
        HealthCanvas.gameObject.SetActive(false);
        MessageBrokerHolder.GameActions.Publish(new M_EntityDeath(this));
        
        await AnimatorController.Death();
        await HideEntity();

        Die();
    }

    public override void Selebrate()
    {
        StateMaсhine.SwitchState<EnemySelebrateState>();
        BallHolder.LostBall();
    }

    [Button]
    public override void Die()
    {
        base.Die();
        OnDestroyed?.Invoke(this);
    }
}