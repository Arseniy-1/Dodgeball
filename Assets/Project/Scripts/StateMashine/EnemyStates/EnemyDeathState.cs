using UnityEngine;

public class EnemyDeathState : EntityDeathState
{
    public EnemyDeathState(AnimatorController animatorController, CollisionHandler collisionHandler,
        Collider collider, BallHolder ballHolder,BallThrower ballThrower) 
        : base(animatorController, collisionHandler, collider, ballHolder, ballThrower)
    {
    }
}