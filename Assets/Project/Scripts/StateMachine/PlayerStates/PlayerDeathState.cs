using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDeathState : EntityDeathState
    {
        public PlayerDeathState(AnimatorController animatorController,  CollisionHandler collisionHandler,
            Collider collider, BallHolder ballHolder, BallThrower ballThrower) 
            : base(animatorController, collisionHandler, collider, ballHolder, ballThrower) { }
    }
}