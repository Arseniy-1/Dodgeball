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

    public virtual void Initialize(Collider squadZone, List<Entity> teamates, Ball ball)
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        
        SquadZone = squadZone;
        Teamates = teamates;
        _ball = ball;
    }

    protected virtual void Update()
    {
        CurrentState = StateMashine._currentState.ToString();
        StateMashine.Update();
    }
}