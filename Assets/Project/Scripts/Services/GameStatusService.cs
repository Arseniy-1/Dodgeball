using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GameStatusService
{
    private static GameStatusService _instance;
    public static GameStatusService Instance => _instance ??= new GameStatusService();

    private Ball _ball;
    private Entity _currentHolder;
    private Collider _currentZone;

    public event Action<Entity> OnHolderChanged;
    public event Action<Collider> OnZoneChanged;

    public Ball CurrentBall => _ball;

    public Entity CurrentHolder
    {
        get => _currentHolder;
        private set
        {
            if (ReferenceEquals(_currentHolder, value))
                return;

            _currentHolder = value;
        }
    }

    public Collider CurrentZone
    {
        get => _currentZone;
        private set
        {
            if (ReferenceEquals(_currentZone, value))
                return;

            _currentZone = value;
        }
    }

    private GameStatusService()
    {
    }

    public void Initialize(Ball ball)
    {
        ClearHolder();
        _ball = ball;
    }

    public void SetCurrentZone(Collider zone)
    {
        CurrentZone = zone;
        OnZoneChanged?.Invoke(CurrentZone);
    }
    
    public void ClearCurrentZone()
    {
        CurrentZone = null;
    }
    
    public void SetHolder(Entity newHolder)
    {
        CurrentHolder = newHolder;
        OnHolderChanged?.Invoke(CurrentHolder);
    }

    public void ClearHolder()
    {
        CurrentHolder = null;
        OnHolderChanged?.Invoke(CurrentHolder);
    }
}