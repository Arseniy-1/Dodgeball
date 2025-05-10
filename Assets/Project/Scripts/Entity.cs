using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class Entity : MonoBehaviour
{
    [SerializeField] protected BallThrower BallThrower;
    [SerializeField] protected BallHolder BallHolder;
    [SerializeField] protected CollisionHandler CollisionHandler;
    [SerializeField] protected TargetScanner TargetScanner;
    [SerializeField] protected Mover Mover;
    [SerializeField] protected GroundChecker GroundChecker;
    [SerializeField] protected Health Health;
    [SerializeField] protected List<Entity> Teamates;

    public string CurrentState;

    protected CompositeDisposable CompositeDisposable = new CompositeDisposable();
    protected TargetProvider TargetProvider = new TargetProvider();
    protected Collider SquadZone;
    protected Collider Collider;
    protected Rigidbody Rigidbody;

    protected StateMashine StateMashine;

    private Ball _ball;
    
    // private void Awake()
    // {
    // }

    private void OnEnable()
    {
        Health.LostHealth += Die;
    }

    private void OnDisable()
    {
        Health.LostHealth -= Die;
    }

    public virtual void Initialize(Collider squadZone, List<Entity> teamates, Ball ball)
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        
        SquadZone = squadZone;
        Teamates = teamates;
        Health.Initialize(CollisionHandler);
        _ball = ball;
    }

    protected virtual void Update()
    {
        CurrentState = StateMashine._currentState.ToString();
        StateMashine.Update();
    }

    protected virtual void Die()
    {
        MessageBrokerHolder.GameActions.Publish(new M_EntityDeath(this));
        gameObject.SetActive(false);
    }
}