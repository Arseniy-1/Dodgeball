using UnityEngine;

public class PlayerJumpState : IState
{
    private readonly PlayerStats _playerStats;
    private readonly Rigidbody _rigidbody;
    private readonly GroundChecker _groundChecker;

    private IStateSwitcher _stateSwitcher;
    
    private bool _hasLanded = false;
    private float _stunTimer = 0f;

    public PlayerJumpState(PlayerStats playerStats, Rigidbody rigidbody, GroundChecker groundChecker)
    {
        _playerStats = playerStats;
        _rigidbody = rigidbody;
        _groundChecker = groundChecker;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
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
