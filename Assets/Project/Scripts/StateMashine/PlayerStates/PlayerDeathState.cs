using UnityEngine;

public class PlayerDeathState : EntityDeathState
{
    public PlayerDeathState(AnimatorController animatorController,  CollisionHandler collisionHandler, Collider collider, BallHolder ballHolder) : base(animatorController, collisionHandler, collider, ballHolder)
    {
    }
}