using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ObjectPool
{
    [Serializable]
    public class Spawner<T> where T : MonoBehaviour, IDestoyable<T>
    {
        [SerializeField] protected int StartAmount = 5;

        protected T Prefab;
        protected Pool<T> Pool;
        
        private List<T> _spawned = new();

        public void DisableSpawned()
        {
            for (int i = _spawned.Count - 1; i >= 0; i--)
            {
                _spawned[i].Die();
            }
        }

        public T Spawn()
        {
            T spawnedObject = Pool.Get();

            spawnedObject.OnDestroyed += OnSpawnedDestroyed;
            _spawned.Add(spawnedObject);

            return spawnedObject;
        }

        protected void OnSpawnedDestroyed(T spawnableObject)
        {
            spawnableObject.OnDestroyed -= OnSpawnedDestroyed;
            _spawned.Remove(spawnableObject);

            Pool.Release(spawnableObject);
        }
    }
}