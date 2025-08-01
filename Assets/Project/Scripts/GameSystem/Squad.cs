using System;
using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using UnityEngine;

namespace Project.Scripts.GameSystem
{
    public class Squad : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<Entity> _entities;

        [SerializeField] private Collider _squadZone;
        
        public event Action<Squad> LostPlayers;
        
        public List<Transform> SpawnPoints => _spawnPoints;
        public Collider SquadZone => _squadZone;
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                if (ball.Rigidbody.isKinematic)
                    return;

                GameStatusService.Instance.ClearCurrentZone();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                if (ball.Rigidbody.isKinematic)
                    return;

                if (GameStatusService.Instance.CurrentZone != null)
                    return;

                GameStatusService.Instance.SetCurrentZone(_squadZone);
            }
        }
        
        private void OnDestroy()
        {
            foreach (var entity in _entities)
            {
                if (entity is Enemy enemy)
                    enemy.Destroyed -= OnDestroyed;
                else if (entity is Player player)
                    player.Destroyed -= OnDestroyed;
            }
        }

        public void Initialize(List<Entity> entities)
        {
            _entities = entities;

            foreach (var entity in _entities)
            {
                if (entity is Enemy enemy)
                    enemy.Destroyed += OnDestroyed;
                else if (entity is Player player)
                    player.Destroyed += OnDestroyed;
            }
        }

        public void Celebrate()
        {
            foreach (var entity in _entities)
            {
                entity.Celebrate();
            }
        }

        private void OnDestroyed(Entity entity)
        {
            if (_entities.Contains(entity))
                _entities.Remove(entity);

            if (_entities.Count == 0)
                LostPlayers?.Invoke(this);
        }
    }
}