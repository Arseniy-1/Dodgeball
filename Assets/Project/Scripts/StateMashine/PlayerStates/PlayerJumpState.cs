using UnityEngine;

public class PlayerJumpState : IState
{
    private readonly PlayerStats _playerStats;
    private readonly Rigidbody _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    
    private IStateSwitcher _stateSwitcher;

    private bool _hasLanded = false;
    private float _stunTimer = 0f;

    public PlayerJumpState(PlayerStats playerStats, Rigidbody rigidbody, GroundChecker groundChecker,
        CollisionHandler collisionHandler, Collider collider)
    {
        _playerStats = playerStats;
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
        _collider.enabled = false;

        if (_rigidbody != null)
            _rigidbody.velocity = Vector3.zero;

        _rigidbody.AddForce(Vector3.up * _playerStats.JumpForce, ForceMode.Force);
        _hasLanded = false;
        _stunTimer = 0f;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (!_hasLanded)
        {
            if (_groundChecker.IsGrounded && Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
            {
                _hasLanded = true;
                _stunTimer = _playerStats.JumpStunTime;
            }
        }
        else
        {
            _stunTimer -= Time.deltaTime;

            if (_stunTimer <= 0f)
                _stateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}