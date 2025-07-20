using System;
using Project.Scripts.Entities;

namespace Project.Scripts.ObjectPool.Entity
{
    [Serializable]
    public class PlayerSpawner : Spawner<Player>
    {
        public PlayerSpawner(Player playerPrefab)
        {
            Prefab = playerPrefab;
            Pool = new PlayerPool(Prefab, StartAmount);
        }
    }
}