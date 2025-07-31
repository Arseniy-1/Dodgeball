using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerMoveState : EntityMoveState
    {
        private readonly Player _player;
        private readonly List<Entity> _teammates;

        public PlayerMoveState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
            _player = (Player)dataHolder.Entity;
            _teammates = dataHolder.Teammates;
        }

        protected override void OnBallDetected(Ball ball)
        {
            StateDataHolder.BallHolder.EquipBall(ball, _player);
            StateSwitcher.SwitchState<PlayerAttackState>();
        }

        protected override void OnBallZoneChanged(Collider zone)
        {
            if (zone != StateDataHolder.SquadZone)
            {
                StateSwitcher.SwitchState<PlayerDodgeReadyState>();
            }
        }

        protected override void OnHolderChanged(Entity entity)
        {
            if (entity == null)
                return;
        
            if (_teammates.Contains(entity) == false)
                StateSwitcher.SwitchState<PlayerDodgeReadyState>();
            else if (entity != _player)
                StateSwitcher.SwitchState<PlayerIdleState>();
        }
    }
}