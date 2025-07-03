using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BallService
{
    private static BallService _instance;
    public static BallService Instance => _instance ??= new BallService();
    
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
            OnHolderChanged?.Invoke(value);
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
            OnZoneChanged?.Invoke(value);
        }
    }

    private BallService() { }

    public void Initialize(Ball ball)
    {
        _ball = ball;
        
        _ball.OnTriggerEnterAsObservable()
            .Subscribe(collider => 
            {
                if (collider.TryGetComponent<Squad>(out _))
                {
                    CurrentZone = collider;
                }
            });

        _ball.OnTriggerExitAsObservable()
            .Subscribe(collider => 
            {
                if (ReferenceEquals(collider, CurrentZone))
                {
                    CurrentZone = null;
                }
            });

        _ball.OnCollisionEnterAsObservable()
            .Subscribe(collision =>
            {
                if (collision.gameObject.TryGetComponent<Entity>(out var entity))
                {
                    SetHolder(entity);
                }
            });
    }

    public void SetHolder(Entity newHolder)
    {
        CurrentHolder = newHolder;
        OnHolderChanged?.Invoke(newHolder);
    }

    public void ClearHolder()
    {
        CurrentHolder = null;
    }
}