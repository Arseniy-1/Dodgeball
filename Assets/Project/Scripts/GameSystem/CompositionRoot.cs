using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Messages;
using Project.Scripts.ObjectPool;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using Project.Scripts.SavesSystem;
using Project.Scripts.Services;
using UniRx;
using UnityEngine;
using YG;

namespace Project.Scripts.GameSystem
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private Player _playerPrefab;

        [SerializeField] private UIHandler _uiHandler;
        [SerializeField] private RewardService _rewardService;
        [SerializeField] private EffectHolder _effectHandler;
        [SerializeField] private Saves _saves;
        [SerializeField] private MapFactory _mapFactory;

        [SerializeField] private int _maxWinRankAmount = 40;
        [SerializeField] private int _minWinRankAmount = 15;
        [SerializeField] private int _maxLoseRankAmount = 10;
        [SerializeField] private int _minLoseRankAmount = 3;

        private bool _rewardRaised = false;
        private RankHolder _rankHolder;
        private CompositeDisposable _disposable;

        private void Awake()
        {
            var entityCreator = new EntityCreator();
            _rewardService.Initialize();
            _rankHolder = new RankHolder();
            _rankHolder.Initialize();
            _disposable = new();

            _uiHandler.Initialize(_rewardService, _rankHolder);
            _effectHandler.Initialize();

            var playerSpawner = new Spawner<Player>(_playerPrefab);
            var enemySpawners = new List<Spawner<Enemy>>();

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                Spawner<Enemy> enemySpawner = new Spawner<Enemy>(_enemyPrefabs[i]);
                enemySpawners.Add(enemySpawner);
            }

            _mapFactory.Initialize(entityCreator, playerSpawner, enemySpawners);

            LocalizationManager.Language = YG2.lang;
            YG2.lang = YG2.lang;
            YG2.SwitchLanguage(YG2.lang);
        }

        private void OnEnable()
        {
            MessageBrokerHolder.GameActions
                .Receive<M_GameOver>()
                .Subscribe(message => HandleGameOverWrapper(message.IsPlayerWin))
                .AddTo(_disposable);

            _uiHandler.StartButtonPressed += StartGame;
            _uiHandler.RankCanvasClosed += HandleRankCanvasClose;
            _rankHolder.RankRaised += HandleRankRaised;
            _uiHandler.Enable();
        }

        private void OnDisable()
        {
            _disposable.Dispose();

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
            _mapFactory.CreateMap();
            GameStatusService.Instance.Initialize(_mapFactory.BallInstance);
            _mapFactory.ArenaInstance.Initialize(_mapFactory.BallInstance);
        }

        private void StartGame()
        {
            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());
        }

        private void HandleGameOverWrapper(bool isWin)
        {
            int rankAmount =
                isWin
                    ? Random.Range(_minWinRankAmount, _maxWinRankAmount)
                    : Random.Range(_minLoseRankAmount, _maxLoseRankAmount);

            _rankHolder.IncreaseRank(rankAmount);
            HandleGameOver().Forget();
        }

        private async UniTaskVoid HandleGameOver()
        {
            await _uiHandler.GameOver();

            if (_rewardRaised)
            {
                _uiHandler.GiveReward();
                _rewardRaised = false;   
            }
        }

        private void HandleRankCanvasClose()
        {
            _mapFactory.ClearEntities();
            _mapFactory.ArenaInstance.Initialize(_mapFactory.BallInstance);
            PrepareMap();
        }

        private void HandleRankRaised()
        {
            _rewardRaised = true;
        }
    }
}