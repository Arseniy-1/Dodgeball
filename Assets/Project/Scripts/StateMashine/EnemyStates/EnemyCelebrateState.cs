using System.Collections.Generic;

public class EnemyCelebrateState : EntityCelebrateState
{
    public EnemyCelebrateState(
        Enemy enemy, AnimatorController animatorController, BallHolder ballHolder,
        BallThrower ballThrower, CollisionHandler collisionHandler, List<Entity> teammates)
        : base(enemy, animatorController, ballHolder, ballThrower, collisionHandler, teammates)
    {
    }
}