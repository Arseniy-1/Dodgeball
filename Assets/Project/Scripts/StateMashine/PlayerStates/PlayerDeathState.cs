using UnityEngine;

public class PlayerDeathState : EntityDeathState
{
    public PlayerDeathState(AnimatorController animatorController,  CollisionHandler collisionHandler, Collider collider) : base(animatorController, collisionHandler, collider)
    {
    }
}