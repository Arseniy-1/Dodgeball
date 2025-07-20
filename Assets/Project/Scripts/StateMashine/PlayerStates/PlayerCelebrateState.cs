using System.Collections.Generic;

public class PlayerCelebrateState : EntityCelebrateState
{
    private readonly PlayerInputController _inputController;
    
    public PlayerCelebrateState(
        Player player, AnimatorController animatorController, BallHolder ballHolder,
        BallThrower ballThrower, CollisionHandler collisionHandler, PlayerInputController playerInputController, List<Entity> teammates)
        : base(player, animatorController, ballHolder, ballThrower, collisionHandler, teammates)
    {
        _inputController = playerInputController;
    }

    public override void Enter()
    {
        base.Enter();
        _inputController.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        _inputController.enabled = true;
    }
}