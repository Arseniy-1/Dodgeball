using System.Collections.Generic;

public class PlayerSelebrateState : EntitySelebrateState
{
    public PlayerSelebrateState(
        Player player,
        AnimatorController animatorController,
        List<Entity> teammates)
        : base(player, animatorController, teammates)
    {
    }
}