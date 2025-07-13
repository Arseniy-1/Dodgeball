using UnityEngine;

public class PlayerDeathState : EntityDeathState
{
    public PlayerDeathState(AnimatorController animatorController,  CollisionHandler collisionHandler,
        Collider collider, BallHolder ballHolder, BallThrower ballThrower) 
        : base(animatorController, collisionHandler, collider, ballHolder, ballThrower)
    {
    }
}