using System.Collections.Generic;

public class EnemyCelebrateState : EntityCelebrateState
{
    public EnemyCelebrateState(
        Enemy enemy,
        AnimatorController animatorController,
        List<Entity> teammates)
        : base(enemy, animatorController, teammates)
    {
    }
}