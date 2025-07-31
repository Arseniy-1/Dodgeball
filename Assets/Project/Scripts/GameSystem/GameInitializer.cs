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

namespace Project.Scripts.GameSystem
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private Player _playerPrefab;

        [SerializeField] private UIHandler _uiHandler;
        [SerializeField] private RewardService _rewardService;
        [SerializeField] private EffectHolder _effectHolder;
        [SerializeField] private SavesSystem.Saves _saves;
        [SerializeField] private MapFactory _mapFactory;

        private bool _rewardRaised = false;
        private RankHolder _rankHolder;

        private void Awake()
        {
            var entityCreator = new EntityCreator();
            _rewardService.Initialize();
            _rankHolder = new RankHolder();
            _rankHolder.Initialize();

            _uiHandler.Initialize(_rewardService, _rankHolder);
            _effectHolder.Initialize();

            var playerSpawner = new PlayerSpawner(_playerPrefab);
            var enemySpawners = new List<EnemySpawner>();

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                EnemySpawner enemySpawner = new EnemySpawner(_enemyPrefabs[i]);
                enemySpawners.Add(enemySpawner);
            }

            _mapFactory.Initialize(entityCreator, playerSpawner, enemySpawners);

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

        private void OnDestroy()
        {
            _effectHolder.Dispose();
        }

        private void PrepareMap()
        {
            _mapFactory.CreateMap();
            GameStatusService.Instance.Initialize(_mapFactory.BallInstance);
            _mapFactory.MatchManagerInstance.Initialize(_mapFactory.BallInstance);
        }

        private void StartGame()
        {
            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());
            _mapFactory.MatchManagerInstance.GameOver += OnGameOver;
        }


        private void OnGameOver(int rankAmount)
        {
            _mapFactory.MatchManagerInstance.GameOver -= OnGameOver;
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
            _mapFactory.ClearEntities();
            _mapFactory.MatchManagerInstance.Initialize(_mapFactory.BallInstance);
        }

        private void HandleRankRaised()
        {
            _rewardRaised = true;
        }
    }
}