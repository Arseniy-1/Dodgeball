using System;
using Project.Scripts.Entities;

namespace Project.Scripts.ObjectPool.Entity
{
    [Serializable]
    public class PlayerSpawner : Spawner<Player>
    {
        public PlayerSpawner(Player playerPrefab) 
            : base(playerPrefab)
        {
        }
    }
}