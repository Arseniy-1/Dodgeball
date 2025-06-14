using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : EntityMoveState
{
    private readonly Player _player;
    private readonly List<Entity> _teammates;
    private readonly PlayerStats _stats;

    public PlayerMoveState(Player player, AnimatorController animatorController, List<Entity> teammates,
        PlayerStats stats, CollisionHandler collisionHandler, Collider squadZone,
        BallHolder ballHolder, Ball ball, Collider collider)
        : base(player, animatorController, collisionHandler, squadZone, ballHolder, ball, collider)
    {
        _player = player;
        _teammates = teammates;
        _stats = stats;
    }

    protected override float GetRotationSpeed() => _stats.RotationSpeed;
    protected override float GetMoveSpeed() => _stats.RunSpeed;

    protected override void OnBallDetected(Ball ball)
    {
        BallHolder.EquipBall(ball);
        StateSwitcher.SwitchState<PlayerAttackState>();
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone != SquadZone)
            StateSwitcher.SwitchState<PlayerDodgeState>();
    }

    protected override void HandleBallTaken(Entity entity)
    {
        if (_teammates.Contains(entity) == false)
        {
            StateSwitcher.SwitchState<PlayerDodgeState>();
        }
        else if (entity != _player)
        {
            StateSwitcher.SwitchState<PlayerIdleState>();
        }
    }
}