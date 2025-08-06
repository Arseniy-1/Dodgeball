using System;
using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.ObjectPool;

namespace Project.Scripts.GameSystem
{
    public class EntityCreator
    {
        public void FillPlayerSquad(Spawner<Player> playerSpawner, Squad squad)
        {
            FillSquad(playerSpawner.Spawn, squad);
        }

        public void FillEnemySquad(Spawner<Enemy> enemySpawner, Squad squad)
        {
            FillSquad(enemySpawner.Spawn, squad);
        }

        private void FillSquad<T>(Func<T> spawnMethod, Squad squad) where T : Entity
        {
            List<Entity> entities = new List<Entity>();

            for (int i = 0; i < squad.SpawnPoints.Count; i++)
            {
                T entity = spawnMethod();
                entity.transform.position = squad.SpawnPoints[i].position;
                entities.Add(entity);
            }

            foreach (var entity in entities)
                entity.Initialize(squad.SquadZone, entities);

            squad.Initialize(entities);
        }
    }
}