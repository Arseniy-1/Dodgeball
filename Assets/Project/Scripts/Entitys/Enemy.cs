using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : Entity, IDestoyable<Enemy>
{
    [SerializeField] private EnemyConfig enemyConfig;
    
    private List<IState> _enemyStates = new();
    
    public event Action<Enemy> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        base.Initialize(squadZone, teammates, ball);
        BallThrower.Initialize(enemyConfig);
        
        foreach (var state in _enemyStates)
        {
            if (state is IDisposable disposable)
                disposable.Dispose();
        }
        
        _enemyStates.Clear();
        
        _enemyStates = new List<IState>
        {
            new EnemyPrepareState(this, AnimatorController, TargetScanner, Teammates),
            new EnemyCelebrateState(this, AnimatorController,BallHolder, BallThrower, CollisionHandler, Teammates),
            new EnemyIdleState(this,AnimatorController, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, enemyConfig, Teammates),
            new EnemyMoveState(this, AnimatorController,Teammates, enemyConfig, CollisionHandler, SquadZone, BallHolder, ball, Collider, Mover),
            new EnemyDodgeState(this, AnimatorController, ball, Mover, SquadZone, Rigidbody, enemyConfig),
            new EnemyAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, enemyConfig),
            new EnemyJumpState(AnimatorController, CollisionHandler, HitCheker, Collider),
            new EnemyDeathState(AnimatorController, CollisionHandler, Collider, BallHolder, BallThrower)
        };
        
        StateMachine = new StateMaсhine(_enemyStates);

        foreach (var state in _enemyStates)
            state.Initialize(StateMachine);

        Reset();
    }
    
    [Button]
    protected override async void HandleLostHealth()
    {
        StateMachine.SwitchState<EnemyDeathState>();
        HealthCanvas.gameObject.SetActive(false);
        EffectID.Death.PlayEffect(transform);
        AudioID.Dead.PlayOneShot();
        
        await AnimatorController.Death();
        await HideEntity();

        Die();
    }

    public override void Celebrate()
    {
        StateMachine.SwitchState<EnemyCelebrateState>();
        BallHolder.LostBall();
    }

    [Button]
    public override void Die()
    {
        base.Die();
        OnDestroyed?.Invoke(this);
    }
}