using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using UnityEngine.Video;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected BallThrower BallThrower;
    [SerializeField] protected BallHolder BallHolder;
    [SerializeField] protected CollisionHandler CollisionHandler;
    [SerializeField] protected TargetScanner TargetScanner;
    [SerializeField] protected Mover Mover;
    [SerializeField] protected GroundChecker GroundChecker;
    [SerializeField] protected Health Health;
    [SerializeField] protected List<Entity> Teammates;

    public string CurrentState;

    protected CompositeDisposable CompositeDisposable = new CompositeDisposable();
    protected TargetProvider TargetProvider = new TargetProvider();
    [SerializeField] protected Collider SquadZone;
    protected Collider Collider;
    protected Rigidbody Rigidbody;

    protected StateMashine StateMashine;

    [SerializeField] private Ball _ball;
    private bool _isEnabled = true;

    private void OnEnable()
    {
        Health.LostHealth += Die;
    }

    private void OnDisable()
    {
        Health.LostHealth -= Die;
    }

    public virtual void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        Teammates = teammates;
        SquadZone = squadZone;
        Health.Initialize(CollisionHandler);
        _ball = ball;
    }

    public virtual void Reset()
    {
        CollisionHandler.enabled = true;
        Collider.enabled = true;
        Health.Reset();
        BallHolder.LostBall();
        _isEnabled = true;
    }

    protected virtual void Update()
    {
        if (_isEnabled)
            StateMashine.Update();

        CurrentState = StateMashine._currentState.ToString();
    }

    [Button]
    protected virtual void Die()
    {
        _isEnabled = false;
    }
}