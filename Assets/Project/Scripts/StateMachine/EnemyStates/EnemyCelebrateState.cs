using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyCelebrateState : EntityCelebrateState
    {
        public EnemyCelebrateState(
            Enemy enemy, AnimatorController animatorController, BallHolder ballHolder,
            BallThrower ballThrower, CollisionHandler collisionHandler, List<Entity> teammates)
            : base(enemy, animatorController, ballHolder, ballThrower, collisionHandler, teammates) { }
    }
}