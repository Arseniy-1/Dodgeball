using UnityEngine;
using System.Threading.Tasks;

public class PlayerJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly PlayerStats _playerStats;
    private readonly GroundChecker _groundChecker;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;

    private IStateSwitcher _stateSwitcher;

    public PlayerJumpState(PlayerStats playerStats, AnimatorController animatorController, GroundChecker groundChecker,
        CollisionHandler collisionHandler, Collider collider)
    {
        _animatorController = animatorController;
        _playerStats = playerStats;
        _groundChecker = groundChecker;
        _collisionHandler = collisionHandler;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;

        Jump();
    }
    
    public void Exit()
    {
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    private async Task Jump()
    {
        await _animatorController.Dodge();
        _stateSwitcher.SwitchState<PlayerDodgeState>();
    }
    
    public void Update()
    {
        
    }
}