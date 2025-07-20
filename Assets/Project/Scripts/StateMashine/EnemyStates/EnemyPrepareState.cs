using System.Collections.Generic;

public class EnemyPrepareState : EntityPrepareState
{
    public EnemyPrepareState(Enemy enemy, AnimatorController animatorController, TargetScanner targetScanner, List<Entity> teammates)
        : base(enemy, animatorController, targetScanner, teammates) { }

    protected override void HandleStartGame()
    {
        StateSwitcher.SwitchState<EnemyIdleState>();
    }
}