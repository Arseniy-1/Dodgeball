using UniRx;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Serialization;

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
    public List<string> CurrentStates = new List<string>();

    protected CompositeDisposable CompositeDisposable = new CompositeDisposable();
    protected TargetProvider TargetProvider = new TargetProvider();
    [SerializeField] protected Collider SquadZone;
    protected Collider Collider;
    protected Rigidbody Rigidbody;

    protected StateMaсhine StateMaсhine;

    [SerializeField] protected Ball Ball;

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
        Ball = ball;
    }

    public virtual void Reset()
    {
        CollisionHandler.enabled = true;
        Collider.enabled = true;
        Health.Reset();
        BallHolder.LostBall();
    }

    protected virtual void Update()
    {
        StateMaсhine.Update();

        CurrentState = StateMaсhine._currentState.ToString();

        CurrentStates.Clear();
        for (int i = 0; i < StateMaсhine._states.Count; i++)
            CurrentStates.Add(StateMaсhine._states[i].ToString());
    }

    [Button]
    public virtual void Die()
    {
        StateMaсhine.Dispose();
    }
}