using UnityEngine;

public class EnemyJumpState : IState
{
    private readonly EnemyStats _enemyStats;
    private readonly Rigidbody _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private Ball _ball;

    private IStateSwitcher _stateSwitcher;

    private bool _hasLanded = false;
    private float _stunTimer = 0f;

    public EnemyJumpState(EnemyStats enemyStats, Rigidbody rigidbody, GroundChecker groundChecker,
        CollisionHandler collisionHandler, Collider collider)
    {
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

        if (_rigidbody != null)
            _rigidbody.velocity = Vector3.zero;

        _rigidbody.AddForce(Vector3.up * _enemyStats.JumpForce, ForceMode.Force);
        _hasLanded = false;
        _stunTimer = 0f;
    }

    public void Exit()
    {
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    public void Update()
    {
        if (!_hasLanded)
        {
            if (_groundChecker.IsGrounded && Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
            {
                _hasLanded = true;
                _stunTimer = _enemyStats.JumpStunTime;
            }
        }
        else
        {
            _stunTimer -= Time.deltaTime;

            if (_stunTimer <= 0f){}
                _stateSwitcher.SwitchState<EnemyDodgeState>();
        }
    }
}