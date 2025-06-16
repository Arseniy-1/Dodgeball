using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public abstract class EntitySelebrateState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly Entity _entity;
    private readonly List<Entity> _teammates;
    private readonly Rotator _rotator;

    protected EntitySelebrateState(
        Entity entity,
        AnimatorController animatorController,
        List<Entity> teammates)
    {
        _entity = entity;
        _animatorController = animatorController;
        _teammates = teammates;
        _rotator = new Rotator();
    }

    public virtual void Enter()
    {
        _animatorController.Selebrate();

        Transform targetTransform = GetTargetTransform();
        _rotator.RotateToTarget(targetTransform, _entity.transform);
    }

    public virtual void Exit() { }
    
    public void Initialize(IStateSwitcher stateSwitcher)
    {
    }

    public virtual void Update() { }

    private Transform GetTargetTransform()
    {
        List<Entity> otherTeammates = _teammates.ToList();
        otherTeammates.Remove(_entity);

        if (otherTeammates.Count > 0)
        {
            int randomIndex = Random.Range(0, otherTeammates.Count);
            
            return otherTeammates[randomIndex].transform;
        }

        return Camera.current?.transform;
    }
}
