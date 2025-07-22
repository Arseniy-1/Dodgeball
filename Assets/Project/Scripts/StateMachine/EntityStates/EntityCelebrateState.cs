using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using UnityEngine;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityCelebrateState : IState
    {
        private readonly AnimatorController _animatorController;
        private readonly Entity _entity;
        private readonly BallHolder _ballHolder;
        private readonly BallThrower _ballThrower;
        private readonly CollisionHandler _collisionHandler;
        private readonly List<Entity> _teammates;
        private readonly Rotator _rotator;

        protected EntityCelebrateState(
            Entity entity,
            AnimatorController animatorController,
            BallHolder ballHolder,
            BallThrower ballThrower,
            CollisionHandler collisionHandler,
            List<Entity> teammates)
        {
            _entity = entity;
            _animatorController = animatorController;
            _ballHolder = ballHolder;
            _ballThrower = ballThrower;
            _collisionHandler = collisionHandler;
            _teammates = teammates;
            _rotator = new Rotator();
        }

        public virtual void Enter()
        {
            _ballHolder.LostBall();
            _ballHolder.enabled = false;
            _ballThrower.enabled = false;
            _collisionHandler.enabled = false;
        
            _animatorController.Celebrate();

            Transform targetTransform = GetTargetTransform();
            _rotator.RotateToTarget(targetTransform, _entity.transform);
        }

        public virtual void Exit()
        {
            _ballHolder.enabled = true;
            _ballThrower.enabled = true;
            _collisionHandler.enabled = true;
        }
    
        public void Initialize(IStateSwitcher stateSwitcher)
        {
        }

        public virtual void Update()
        {
        }

        private Transform GetTargetTransform()
        {
            List<Entity> otherTeammates = _teammates.ToList();
            otherTeammates.Remove(_entity);

            if (otherTeammates.Count > 0)
            {
                int randomIndex = Random.Range(0, otherTeammates.Count);
            
                return otherTeammates[randomIndex].transform;
            }

            return Camera.main?.transform;
        }
    }
}