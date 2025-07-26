using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Messages;
using Project.Scripts.ObjectPool.Entity;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using Project.Scripts.Services;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Project.Scripts.CompositionRootSystem
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private List<Arena> _arenaPrefabs;
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private UIHandler _uiHandler;
        [SerializeField] private RewardService _rewardService;
        [SerializeField] private EffectHandler _effectHandler;
        [SerializeField] private Saves.Saves _saves;

        private bool _rewardRaised = false;
        private RankHolder _rankHolder;
        
        private EntityCreator _entityCreator;
        private Ball _ballInstance;
        private Arena _arenaInstance;

        private PlayerSpawner _playerSpawner;
        private List<EnemySpawner> _enemySpawners = new ();

        private void Awake()
        {
            _entityCreator = new EntityCreator();
            _rewardService.Initialize();
            _rankHolder = new RankHolder();
            _rankHolder.Initialize();
            
            _uiHandler.Initialize(_rewardService, _rankHolder);
            _effectHandler.Initialize();

            _playerSpawner = new PlayerSpawner(_playerPrefab);

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                EnemySpawner enemySpawner = new EnemySpawner(_enemyPrefabs[i]);
                _enemySpawners.Add(enemySpawner);
            }

            LocalizationManager.Language = YG2.lang;
            YG2.lang = YG2.lang;
            YG2.SwitchLanguage(YG2.lang);
        }

        private void OnEnable()
        {
            _uiHandler.StartButtonPressed += StartGame;
            _uiHandler.RankCanvasClosed += HandleRankCanvasClose;
            _rankHolder.RankRaised += HandleRankRaised;
            _uiHandler.Enable();
        }

        private void OnDisable()
        {
            _uiHandler.StartButtonPressed -= StartGame;
            _uiHandler.RankCanvasClosed -= HandleRankCanvasClose;
            _rankHolder.RankRaised -= HandleRankRaised;
            _uiHandler.Enable();
        }

        private void Start()
        {
            _saves.Initialize(_rankHolder);
            CreateMap();
            GameStatusService.Instance.Initialize(_ballInstance);
            _uiHandler.Start();
        }

        private void StartGame()
        {
            _arenaInstance.Initialize(_ballInstance);

            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());
        }

        private void CreateMap()
        {
            if (_arenaInstance != null)
            {
                ClearEntities();
                Destroy(_arenaInstance.gameObject);
            }

            Arena arenaPrefab = _arenaPrefabs[Random.Range(0, _arenaPrefabs.Count)];

            _arenaInstance = Instantiate(arenaPrefab, transform.position, Quaternion.identity);

            if (_ballInstance != null)
                Destroy(_ballInstance.gameObject);

            const float ballOffsetX = -2.5f;
            const float ballOffsetY = 1f;
            const float ballOffsetZ = -1.5f;
            
            Vector3 ballPosition = new Vector3(
                transform.position.x + ballOffsetX, 
                transform.position.y + ballOffsetY,
                transform.position.z + ballOffsetZ);
            
            _ballInstance = Instantiate(_ballPrefab, ballPosition, Quaternion.identity);

            for (int i = 0; i < _arenaInstance.Squads.Count; i++)
            {
                if (i == 0)
                {
                    _entityCreator.FillPlayerSquad(_playerSpawner, _arenaInstance.Squads[i]);
                }
                else
                {
                    _entityCreator.FillEnemySquad(_enemySpawners[Random.Range(0, _enemySpawners.Count)], _arenaInstance.Squads[i]);
                }
            }

            _arenaInstance.GameOver += HandleGameOverWrapper;
        }

        private void HandleGameOverWrapper(int rankAmount)
        {
            _rankHolder.IncreaseRank(rankAmount);
            HandleGameOver().Forget();
        }

        private async UniTaskVoid HandleGameOver()
        {
            _arenaInstance.GameOver -= HandleGameOverWrapper;

            await _uiHandler.GameOver();

            if (_rewardRaised)
                _uiHandler.GiveReward();
        }

        private void HandleRankCanvasClose()
        {
            ClearEntities();
            CreateMap();
        }

        private void ClearEntities()
        {
            foreach (var enemySpawner in _enemySpawners)
                enemySpawner.DisableSpawned();

            _playerSpawner.DisableSpawned();
        }

        private void HandleRankRaised()
        {
            _rewardRaised = true;
        }
    }
}