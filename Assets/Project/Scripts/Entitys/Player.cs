using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Entity, IDestoyable<Player>
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStats _playerStats;

    public event Action<Player> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teamates, Ball ball)
    {
        base.Initialize(squadZone, teamates, ball);
        BallThrower.Initialize(_playerStats);

        List<IState> playerStates = new List<IState>
        {
            new PlayerIdleState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats,
                CompositeDisposable),
            new PlayerMoveState(this, _playerStats, Mover, CollisionHandler, SquadZone, CompositeDisposable, BallHolder,
                ball),
            new PlayerDodgeState(this, ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats,
                _inputController, CompositeDisposable),
            new PlayerAttackState(this, BallHolder, TargetScanner, TargetProvider, Teamates, _inputController,
                BallThrower),
            new PlayerJumpState(_playerStats, Rigidbody, GroundChecker, CollisionHandler, Collider)
        };

        StateMashine = new StateMashine(playerStates);

        foreach (var state in playerStates)
            state.Initialize(StateMashine);
    }

    public override void Reset()
    {
        Health.Reset();
        StateMashine.SwitchState<PlayerIdleState>();
    }

    [Button]
    protected override void Die()
    {
        OnDestroyed?.Invoke(this);
    }
}