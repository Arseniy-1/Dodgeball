using System;
using System.Collections.Generic;
using Project.Scripts.ObjectPool.Entity;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameSystem
{
    [Serializable]
    public class MapFactory
    {
        [SerializeField] private List<MatchManager> _arenaPrefabs;
        [SerializeField] private Ball _ballPrefab;

        private EntityCreator _entityCreator;
        private PlayerSpawner _playerSpawner;
        private List<EnemySpawner> _enemySpawners;

        public void Initialize(
            EntityCreator entityCreator, 
            PlayerSpawner playerSpawner, 
            List<EnemySpawner> enemySpawners)
        {
            _entityCreator = entityCreator;
            _playerSpawner = playerSpawner;
            _enemySpawners = enemySpawners;
        }
        
        public Ball BallInstance { get; private set; }
        public MatchManager MatchManagerInstance { get; private set; }

        public void CreateMap()
        {
            var position = Vector3.zero;

            if (MatchManagerInstance != null)
            {
                ClearEntities();
                Object.Destroy(MatchManagerInstance.gameObject);
            }

            MatchManager matchManagerPrefab = _arenaPrefabs[Random.Range(0, _arenaPrefabs.Count)];

            MatchManagerInstance = Object.Instantiate(matchManagerPrefab, position, Quaternion.identity);

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

            for (int i = 0; i < MatchManagerInstance.Squads.Count; i++)
            {
                if (i == 0)
                {
                    _entityCreator.FillPlayerSquad(_playerSpawner, MatchManagerInstance.Squads[i]);
                }
                else
                {
                    _entityCreator.FillEnemySquad(_enemySpawners[Random.Range(0, _enemySpawners.Count)],
                        MatchManagerInstance.Squads[i]);
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