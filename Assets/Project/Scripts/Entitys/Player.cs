using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Entity, IDestoyable<Player>
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStats _playerStats;

    private List<IState> _playerStates = new();

    public event Action<Player> OnDestroyed;

    public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        base.Initialize(squadZone, teammates, ball);
        BallThrower.Initialize(_playerStats);

        foreach (var state in _playerStates)
        {
            if (state is IDisposable disposable)
                disposable.Dispose();
        }
        
        _playerStates.Clear();

        _playerStates = new List<IState>
        {
            new PlayerIdleState(this, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats),
            new PlayerMoveState(this, Teammates, _playerStats, CollisionHandler, SquadZone, BallHolder, Ball, Collider),
            new PlayerDodgeState(this, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats, _inputController),
            new PlayerAttackState(this, BallHolder, TargetScanner, TargetProvider, Teammates, _inputController, BallThrower),
            new PlayerJumpState(_playerStats, Rigidbody, GroundChecker, CollisionHandler, Collider),
        };

        StateMaсhine = new StateMaсhine(_playerStates);

        foreach (var state in _playerStates)
            state.Initialize(StateMaсhine);

        Reset();
    }

    public override void Reset()
    {
        base.Reset();
    }

    [Button]
    public  override void Die()
    {
        base.Die();
        OnDestroyed?.Invoke(this);
    }
}
