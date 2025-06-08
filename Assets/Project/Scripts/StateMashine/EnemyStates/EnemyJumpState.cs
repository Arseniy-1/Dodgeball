using UnityEngine;
using System.Threading.Tasks;

public class EnemyJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly EnemyStats _enemyStats;
    private readonly Rigidbody _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private Ball _ball;

    private IStateSwitcher _stateSwitcher;

    public EnemyJumpState(EnemyStats enemyStats, AnimatorController animatorController, Rigidbody rigidbody,
        GroundChecker groundChecker,
        CollisionHandler collisionHandler, Collider collider)
    {
        _animatorController = animatorController;
        _enemyStats = enemyStats;
        _rigidbody = rigidbody;
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
        _stateSwitcher.SwitchState<EnemyDodgeState>();
    }

    public void Update()
    {
        
    }
}