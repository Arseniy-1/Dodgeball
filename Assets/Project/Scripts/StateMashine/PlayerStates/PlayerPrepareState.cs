using System.Collections.Generic;

public class PlayerPrepareState : EntityPrepareState
{
    public PlayerPrepareState(
        Player player,
        AnimatorController animatorController,
        TargetScanner targetScanner,
        List<Entity> teammates)
        : base(player, animatorController, targetScanner, teammates)
    {
    }

    protected override void HandleStartGame()
    {
        StateSwitcher.SwitchState<PlayerIdleState>();
    }
}