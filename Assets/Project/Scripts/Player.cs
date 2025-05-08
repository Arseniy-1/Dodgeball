using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private PlayerInputController _inputController;
    
    private StateMashine _stateMashine;

    protected override void Initialize(Collider squadZone)
    {
        base.Initialize(squadZone);

        List<IState> playerStates = new List<IState>
        {
            new PlayerIdleState(this),
            new PlayerMoveState(this, CollisionHandler, SquadZone,CompositeDisposable, BallHolder),
            new PlayerDodgeState(this),
            new PlayerAttackState(this, TargetScanner, TargetProvider, Teamates, _inputController, SquadZone, BallThrower)
        };
        
        _stateMashine = new StateMashine(playerStates);
    }
    
    private void Update()
    {
        _stateMashine.Update();
    }
}