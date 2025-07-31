using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using UnityEngine;

public class StateDataHolder
{
    public Entity Entity { get; private set; }
    public CollisionHandler CollisionHandler { get; private set; }
    public Collider Collider { get; private set; }
    public Collider SquadZone { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public AnimatorController AnimatorController { get; private set; }
    public BallHolder BallHolder { get; private set; }
    public TargetScanner TargetScanner { get; private set; }
    public TargetProvider TargetProvider { get; private set; }
    public List<Entity> Teammates { get; private set; }
    public BallThrower BallThrower { get; private set; }
    public Mover Mover { get; private set; }
    public EntityConfig EntityConfig { get; private set; }
    public HitDetector HitDetector { get; private set; }
    public Rotator Rotator { get; private set; }
    public AreaPointSelector AreaPointSelector { get; private set; }

    public StateDataHolder(
        Entity entity,
        CollisionHandler collisionHandler,
        Collider collider,
        Collider squadZone,
        Rigidbody rigidbody,
        AnimatorController animatorController,
        BallHolder ballHolder,
        TargetScanner targetScanner,
        TargetProvider targetProvider,
        List<Entity> teammates,
        BallThrower ballThrower,
        Mover mover,
        EntityConfig entityConfig,
        HitDetector hitDetector)
    {
        Entity = entity;
        CollisionHandler = collisionHandler;
        Collider = collider;
        SquadZone = squadZone;
        Rigidbody = rigidbody;
        AnimatorController = animatorController;
        BallHolder = ballHolder;
        TargetScanner = targetScanner;
        TargetProvider = targetProvider;
        Teammates = teammates;
        BallThrower = ballThrower;
        Mover = mover;
        EntityConfig = entityConfig;
        HitDetector = hitDetector;
        Rotator = new Rotator();
        AreaPointSelector = new AreaPointSelector();
    }
}