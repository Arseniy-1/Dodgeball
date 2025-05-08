using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityStats _entityStats;
    [SerializeField] protected BallThrower BallThrower;
    [SerializeField] protected BallHolder BallHolder;
    [SerializeField] protected CollisionHandler CollisionHandler;
    [SerializeField] protected TargetScanner TargetScanner;
    [SerializeField] protected List<Entity> Teamates;
    
    protected CompositeDisposable CompositeDisposable = new CompositeDisposable();
    protected TargetProvider TargetProvider;
    protected Collider SquadZone;
    
    private StateMashine _stateMashine;

    protected virtual void Initialize(Collider squadZone)
    {
        BallThrower.Initialize(_entityStats);
        SquadZone = squadZone;
    }
    
    private void Update()
    {
        _stateMashine.Update();
    }
}