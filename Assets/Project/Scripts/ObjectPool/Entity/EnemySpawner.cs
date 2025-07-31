using System;
using Project.Scripts.Entities;

namespace Project.Scripts.ObjectPool.Entity
{
    [Serializable]
    public class EnemySpawner : Spawner<Enemy>
    {
        public EnemySpawner(Enemy enemyPrefab)
            : base(enemyPrefab)
        {
        }
    }
}