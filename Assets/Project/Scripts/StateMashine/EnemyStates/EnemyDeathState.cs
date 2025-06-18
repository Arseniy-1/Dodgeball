using UnityEngine;

public class EnemyDeathState : EntityDeathState
{
    public EnemyDeathState(AnimatorController animatorController, CollisionHandler collisionHandler, Collider collider, BallHolder ballHolder) : base(animatorController, collisionHandler, collider, ballHolder)
    {
    }
}