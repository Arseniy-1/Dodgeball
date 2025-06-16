using System.Collections.Generic;

public class EnemySelebrateState : EntitySelebrateState
{
    public EnemySelebrateState(
        Enemy enemy,
        AnimatorController animatorController,
        List<Entity> teammates)
        : base(enemy, animatorController, teammates)
    {
    }
}