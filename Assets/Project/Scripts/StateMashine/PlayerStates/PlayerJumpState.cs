using UnityEngine;

public class PlayerJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly PlayerStats _playerStats;
    private readonly GroundChecker _groundChecker;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;

    private IStateSwitcher _stateSwitcher;

    private float _stunTimer = 0f;

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
        _animatorController.Dodge();
        
        _stunTimer = 2f;
    }

    public void Exit()
    {
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    public void Update()
    {
        _stunTimer -= Time.deltaTime;

        if (_stunTimer <= 0f)
            _stateSwitcher.SwitchState<PlayerDodgeState>();
    }
}