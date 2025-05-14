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

        // Очистка старых состояний
        foreach (var state in _playerStates)
        {
            if (state is IDisposable disposable)
                disposable.Dispose();
        }
        _playerStates.Clear();

        // Создание и сохранение новых состояний
        _playerStates = new List<IState>
        {
            new PlayerIdleState(this, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats,
                CompositeDisposable),
            new PlayerMoveState(this, _playerStats, Mover, CollisionHandler, SquadZone, CompositeDisposable,
                BallHolder, Ball, Collider),
            new PlayerDodgeState(this, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerStats,
                _inputController, CompositeDisposable),
            new PlayerAttackState(this, BallHolder, TargetScanner, TargetProvider, Teammates, _inputController,
                BallThrower),
            new PlayerJumpState(_playerStats, Rigidbody, GroundChecker, CollisionHandler, Collider),
            new PlayerDeathState(this, _playerStats, Mover, CollisionHandler, SquadZone, CompositeDisposable,
                BallHolder, Ball, Collider)
        };

        // Пересоздание StateMashine
        StateMashine?.Dispose(); // если реализовано IDisposable — освободить ресурсы
        StateMashine = new StateMashine(_playerStates);

        // Инициализация состояний
        foreach (var state in _playerStates)
            state.Initialize(StateMashine);

        Reset();
    }

    public override void Reset()
    {
        base.Reset();
    }

    [Button]
    protected override void Die()
    {
        OnDestroyed?.Invoke(this);
        // StateMashine?.SwitchState<PlayerDeathState>();
    }
}
