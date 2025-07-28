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

namespace Project.Scripts.CompositionRootSystem
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private Player _playerPrefab;

        [SerializeField] private UIHandler _uiHandler;
        [SerializeField] private RewardService _rewardService;
        [SerializeField] private EffectHandler _effectHandler;
        [SerializeField] private Saves.Saves _saves;
        [SerializeField] private MapController _mapController;

        private bool _rewardRaised = false;
        private RankHolder _rankHolder;

        private void Awake()
        {
            var entityCreator = new EntityCreator();
            _rewardService.Initialize();
            _rankHolder = new RankHolder();
            _rankHolder.Initialize();

            _uiHandler.Initialize(_rewardService, _rankHolder);
            _effectHandler.Initialize();

            var playerSpawner = new PlayerSpawner(_playerPrefab);
            var enemySpawners = new List<EnemySpawner>();

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                EnemySpawner enemySpawner = new EnemySpawner(_enemyPrefabs[i]);
                enemySpawners.Add(enemySpawner);
            }

            _mapController.Initialize(entityCreator, playerSpawner, enemySpawners);

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
            _uiHandler.Disable();
        }

        private void Start()
        {
            _saves.Initialize(_rankHolder);
            _uiHandler.Start();
            
            PrepareMap();
        }

        private void PrepareMap()
        {
            _mapController.CreateMap();
            GameStatusService.Instance.Initialize(_mapController.BallInstance);
            _mapController.ArenaInstance.Initialize(_mapController.BallInstance);
        }

        private void StartGame()
        {
            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());
            _mapController.ArenaInstance.GameOver += HandleGameOverWrapper;
        }


        private void HandleGameOverWrapper(int rankAmount)
        {
            _mapController.ArenaInstance.GameOver -= HandleGameOverWrapper;
            _rankHolder.IncreaseRank(rankAmount);
            HandleGameOver().Forget();
        }

        private async UniTaskVoid HandleGameOver()
        {
            await _uiHandler.GameOver();

            if (_rewardRaised)
                _uiHandler.GiveReward();
        }

        private void HandleRankCanvasClose()
        {
            _mapController.ClearEntities();
            _mapController.ArenaInstance.Initialize(_mapController.BallInstance);
        }

        private void HandleRankRaised()
        {
            _rewardRaised = true;
        }
    }
}