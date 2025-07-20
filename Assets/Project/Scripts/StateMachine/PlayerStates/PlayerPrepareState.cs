using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.Services.EffectService;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;
using Project.Scripts.Entities;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerPrepareState : EntityPrepareState
    {
        private readonly Transform _playerTransform;
    
        public PlayerPrepareState(Player player, AnimatorController animatorController, TargetScanner targetScanner, List<Entity> teammates)
            : base(player, animatorController, targetScanner, teammates)
        {
            _playerTransform = player.transform;
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