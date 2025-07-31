using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ObjectPool
{
    [Serializable]
    public abstract class Spawner<T> where T : MonoBehaviour, IDestoyable<T>
    {
        [field: SerializeField] protected int StartAmount { get; private set; } = 5;

        protected T Prefab { get; private set; }
        protected Pool<T> Pool { get; private set; }

        private List<T> _spawned = new();

        protected Spawner(T prefab)
        {
            Prefab = prefab;
            Pool = CreatePool();
        }

        protected abstract Pool<T> CreatePool();

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

            spawnedObject.Destroyed += OnSpawnedDestroyed;
            _spawned.Add(spawnedObject);

            return spawnedObject;
        }

        protected void OnSpawnedDestroyed(T spawnableObject)
        {
            spawnableObject.Destroyed -= OnSpawnedDestroyed;
            _spawned.Remove(spawnableObject);

            Pool.Release(spawnableObject);
        }
    }
}