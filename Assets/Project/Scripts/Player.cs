using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStats _playerStats;

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
            new PlayerDodgeState(this, ball, Mover, CollisionHandler, _inputController, SquadZone, Collider, Rigidbody,
                _playerStats,
                CompositeDisposable),
            new PlayerAttackState(this, BallHolder, TargetScanner, TargetProvider, Teamates, _inputController,
                SquadZone, BallThrower),
            new PlayerJumpState(this, _playerStats, Rigidbody, GroundChecker)
        };

        StateMashine = new StateMashine(playerStates);

        foreach (var state in playerStates)
            state.Initialize(StateMashine);
    }

    // protected override void Update()
    // {
    //     StateMashine.Update();
    // }
}