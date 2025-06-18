using UnityEngine;

public class EnemyDeathState : EntityDeathState
{
    public EnemyDeathState(AnimatorController animatorController, CollisionHandler collisionHandler, Collider collider) : base(animatorController, collisionHandler, collider)
    {
    }
}