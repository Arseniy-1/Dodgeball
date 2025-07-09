using System.Collections.Generic;
using UnityEngine;

public class PlayerPrepareState : EntityPrepareState
{
    private Transform _playerTransform;
    
    public PlayerPrepareState(
        Player player,
        AnimatorController animatorController,
        TargetScanner targetScanner,
        List<Entity> teammates)
        : base(player, animatorController, targetScanner, teammates)
    {
        _playerTransform = player.transform;
    }

    public override void Exit()
    {
        EffectID.Pointer.PlayEffect(_playerTransform, true);
    }
    
    protected override void HandleStartGame()
    {
        StateSwitcher.SwitchState<PlayerIdleState>();
    }
}