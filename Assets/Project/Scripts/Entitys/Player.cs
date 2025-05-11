using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Entity, IDestoyable<Player>
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStats _playerStats;
    public event Action<Player> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        base.Initialize(squadZone, teammates, ball);
        BallThrower.Initialize(_playerStats);

        List<IState> playerStates = new List<IState>
        {
            new PlayerIdleState(this, ball, Mover, CollisionHandler, squadZone, Collider, Rigidbody, _playerStats,
                CompositeDisposable),
            new PlayerMoveState(this, _playerStats, Mover, CollisionHandler, squadZone, CompositeDisposable, BallHolder,
                ball, Collider),
            new PlayerDodgeState(this, ball, Mover, CollisionHandler, squadZone, Collider, Rigidbody, _playerStats,
                _inputController, CompositeDisposable),
            new PlayerAttackState(this, BallHolder, TargetScanner, TargetProvider, Teammates, _inputController,
                BallThrower),
            new PlayerJumpState(_playerStats, Rigidbody, GroundChecker, CollisionHandler, Collider)
        };

        StateMashine = new StateMashine(playerStates);

        foreach (var state in playerStates)
            state.Initialize(StateMashine);
        
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        StateMashine.SwitchState<PlayerIdleState>();
    }

    [Button]
    protected override void Die()
    {
        OnDestroyed?.Invoke(this);
    }
}