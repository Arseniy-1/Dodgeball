using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerPrepareState : EntityPrepareState
    {
        private readonly Transform _playerTransform;
    
        public PlayerPrepareState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
            _playerTransform = dataHolder.Entity.transform;
        }

        public override void Exit()
        {
            base.Exit();
            EffectID.Pointer.PlayEffect(_playerTransform, true);
        }
    
        protected override void HandleStartGame()
        {
            StateSwitcher.SwitchState<PlayerIdleState>();
        }
    }
}