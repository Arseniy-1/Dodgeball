using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ObjectPool
{
    [Serializable]
    public class Spawner<T> where T : MonoBehaviour, IDestoyable<T>
    {
        [field: SerializeField] protected int StartAmount { get; private set; } = 0;

        protected T Prefab { get; private set; }
        protected Pool<T> Pool { get; private set; }

        private List<T> _spawned = new();
        private Transform _parent;

        public Spawner(T prefab, Transform parent = null)
        {
            Prefab = prefab;
            Pool = new Pool<T>(prefab, StartAmount);
            _parent = parent;
        }

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

            spawnedObject.Destroyed += OnDestroyed;
            _spawned.Add(spawnedObject);
            
            if(_parent != null)
                spawnedObject.transform.SetParent(_parent);
            
            return spawnedObject;
        }

        protected void OnDestroyed(T spawnableObject)
        {
            spawnableObject.Destroyed -= OnDestroyed;
            _spawned.Remove(spawnableObject);

            Pool.Release(spawnableObject);
        }
    }
}