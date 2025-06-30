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
            new EnemySelebrateState(this, AnimatorController, Teammates),
            new EnemyIdleState(this,AnimatorController, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, enemyConfig),
            new EnemyMoveState(this, AnimatorController, enemyConfig, CollisionHandler, SquadZone, BallHolder, ball, Collider, Mover),
            new EnemyDodgeState(this, AnimatorController, ball, Mover, SquadZone, Rigidbody, enemyConfig),
            new EnemyAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, BallThrower, enemyConfig),
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
        EffectID.Death.PlayEffect(transform);
        AudioID.Dead.PlayOneShot();
        
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