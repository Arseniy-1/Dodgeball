using System;
using Project.Scripts.Entities;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class GameStatusService
    {
        private static GameStatusService _instance;

        private Ball.Ball _ball;
        private Entity _currentHolder;
        private Collider _currentZone;
   
        private GameStatusService()
        {
        }

        public event Action<Entity> HolderChanged;
        public event Action<Collider> ZoneChanged;

        public static GameStatusService Instance => _instance ??= new GameStatusService();
        public Ball.Ball CurrentBall => _ball;
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
        
        public void Initialize(Ball.Ball ball)
        {
            ClearHolder();
            _ball = ball;
        }

        public void SetCurrentZone(Collider zone)
        {
            CurrentZone = zone;
            ZoneChanged?.Invoke(CurrentZone);
        }
    
        public void ClearCurrentZone()
        {
            CurrentZone = null;
        }
    
        public void SetHolder(Entity newHolder)
        {
            CurrentHolder = newHolder;
            HolderChanged?.Invoke(CurrentHolder);
        }

        public void ClearHolder()
        {
            CurrentHolder = null;
            HolderChanged?.Invoke(CurrentHolder);
        }
    }
}