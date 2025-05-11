using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spawner<T> where T : MonoBehaviour, IDestoyable<T>
{
    private List<T> _spawned = new List<T>();
    
    protected T Prefab;
    protected int StartAmount = 5;

    protected Pool<T> Pool;

    public event Action<int, int, int> CountChanged;

    public void DisableSpawned()
    {
        foreach (var spawned in _spawned)
        {
            spawned.OnDestroyed -= OnSpawnedDestroyed;
            Pool.Release(spawned);
        }
        
        _spawned.Clear();
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