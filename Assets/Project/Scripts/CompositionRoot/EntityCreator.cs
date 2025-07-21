using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.ObjectPool.Entity;
using Project.Scripts.Services;

namespace Project.Scripts.CompositionRoot
{
    public class EntityCreator
    {
        public  void FillPlayerSquad(PlayerSpawner playerSpawner, Squad squad)
        {
            List<Entity> players = new List<Entity>();

            for (int i = 0; i < squad.SpawnPoints.Count; i++)
            {
                Player player = playerSpawner.Spawn();
                player.transform.position = squad.SpawnPoints[i].position;

                players.Add(player);
            }

            foreach (var player in players)
                player.Initialize(squad.SquadZone, players, GameStatusService.Instance.CurrentBall);

            squad.Initialize(players);
        }

        public void FillEnemySquad(EnemySpawner enemySpawner, Squad squad)
        {
            List<Entity> enemies = new List<Entity>();

            for (int i = 0; i < squad.SpawnPoints.Count; i++)
            {
                Enemy enemy = enemySpawner.Spawn();
                enemy.transform.position = squad.SpawnPoints[i].position;

                enemies.Add(enemy);
            }

            foreach (var enemy in enemies)
                enemy.Initialize(squad.SquadZone, enemies, GameStatusService.Instance.CurrentBall);

            squad.Initialize(enemies);
        }
    }
}