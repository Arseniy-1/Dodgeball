using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class EntitySelebrateState
{
    private readonly AnimatorController _animatorController;
    private readonly Entity _entity;
    private readonly List<Entity> _teammates;
    private readonly TargetScanner _targetScanner;
    private readonly Rotator _rotator;
    
    protected IStateSwitcher StateSwitcher;

    protected EntitySelebrateState(
        Entity entity,
        AnimatorController animatorController,
        TargetScanner targetScanner,
        List<Entity> teammates)
    {
        _entity = entity;
        _animatorController = animatorController;
        _targetScanner = targetScanner;
        _teammates = teammates;
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _animatorController.Selebrate();
        
        _rotator.RotateToTarget(Camera.current.transform, _entity.transform);
    }

    public virtual void Exit() { }

    public virtual void Update() { }
}