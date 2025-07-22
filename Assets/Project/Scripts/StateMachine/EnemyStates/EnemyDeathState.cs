using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyDeathState : EntityDeathState
    {
        public EnemyDeathState(
            AnimatorController animatorController,
            CollisionHandler collisionHandler,
            Collider collider,
            BallHolder ballHolder,
            BallThrower ballThrower)
            : base(
                animatorController,
                collisionHandler,
                collider,
                ballHolder,
                ballThrower)
        {
        }
    }
}