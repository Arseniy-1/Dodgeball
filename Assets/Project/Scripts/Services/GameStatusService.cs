using System;
using Project.Scripts.Entities;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class GameStatusService
    {
        private static GameStatusService _instance;

        private Scripts.Ball _ball;
        private Entity _currentHolder;
        private Collider _currentZone;
   
        private GameStatusService()
        {
        }

        public event Action<Entity> OnHolderChanged;
        public event Action<Collider> OnZoneChanged;

        public static GameStatusService Instance => _instance ??= new GameStatusService();
        public Scripts.Ball CurrentBall => _ball;
        public bool IsBallFree => _ball.Chargeable.IsCharged == false && _currentHolder == null;

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
        
        public void Initialize(Scripts.Ball ball)
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
}