using System;
using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.GameSystem;
using Project.Scripts.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.CompositionRootSystem
{
    [Serializable]
    public class MapController
    {
        [SerializeField] private List<Arena> _arenaPrefabs;
        [SerializeField] private Ball _ballPrefab;

        private EntityCreator _entityCreator;
        private Spawner<Player> _playerSpawner;
        private List<Spawner<Enemy>> _enemySpawners;

        public void Initialize(
            EntityCreator entityCreator, 
            Spawner<Player> playerSpawner, 
            List<Spawner<Enemy>> enemySpawners)
        {
            _entityCreator = entityCreator;
            _playerSpawner = playerSpawner;
            _enemySpawners = enemySpawners;
        }
        
        public Ball BallInstance { get; private set; }
        public Arena ArenaInstance { get; private set; }

        public void CreateMap()
        {
            var position = Vector3.zero;

            if (ArenaInstance != null)
            {
                ClearEntities();
                Object.Destroy(ArenaInstance.gameObject);
            }

            Arena arenaPrefab = _arenaPrefabs[Random.Range(0, _arenaPrefabs.Count)];

            ArenaInstance = Object.Instantiate(arenaPrefab, position, Quaternion.identity);

            if (BallInstance != null)
                Object.Destroy(BallInstance.gameObject);

            const float ballOffsetX = -2.5f;
            const float ballOffsetY = 1f;
            const float ballOffsetZ = -1.5f;

            Vector3 ballPosition = new Vector3(
                position.x + ballOffsetX,
                position.y + ballOffsetY,
                position.z + ballOffsetZ);

            BallInstance = Object.Instantiate(_ballPrefab, ballPosition, Quaternion.identity);

            for (int i = 0; i < ArenaInstance.Squads.Count; i++)
            {
                if (i == 0)
                {
                    _entityCreator.FillPlayerSquad(_playerSpawner, ArenaInstance.Squads[i]);
                }
                else
                {
                    _entityCreator.FillEnemySquad(_enemySpawners[Random.Range(0, _enemySpawners.Count)],
                        ArenaInstance.Squads[i]);
                }
            }
        }


        public void ClearEntities()
        {
            foreach (var enemySpawner in _enemySpawners)
                enemySpawner.DisableSpawned();

            _playerSpawner.DisableSpawned();
        }
    }
}