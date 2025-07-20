using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : EntityMoveState
{
    private readonly Player _player;
    private readonly List<Entity> _teammates;

    public PlayerMoveState(Player player, AnimatorController animatorController, List<Entity> teammates,
        PlayerConfig playerConfig, CollisionHandler collisionHandler, Collider squadZone,
        BallHolder ballHolder, Collider collider, Mover mover)
        : base(player, animatorController, collisionHandler, squadZone, ballHolder, collider, playerConfig, mover)
    {
        _player = player;
        _teammates = teammates;
    }

    protected override void OnBallDetected(Ball ball)
    {
        BallHolder.EquipBall(ball, _player);
        StateSwitcher.SwitchState<PlayerAttackState>();
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone != SquadZone)
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
    }

    protected override void HandleBallHolderChanged(Entity entity)
    {
        if (entity == null)
            return;
        
        if (_teammates.Contains(entity) == false)
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
        else if (entity != _player)
        {
            StateSwitcher.SwitchState<PlayerIdleState>();
        }
    }
}