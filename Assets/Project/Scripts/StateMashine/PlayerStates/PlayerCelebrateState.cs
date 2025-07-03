using System.Collections.Generic;

public class PlayerCelebrateState : EntityCelebrateState
{
    public PlayerCelebrateState(
        Player player,
        AnimatorController animatorController,
        List<Entity> teammates)
        : base(player, animatorController, teammates)
    {
    }
}