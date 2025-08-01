using System;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.GameSystem;
using Project.Scripts.Messages;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using Project.Scripts.SavesSystem;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.UI;
using Project.Scripts.UI.Canvases;
using Project.Scripts.UI.View;
using UnityEngine;
using YG;
using AudioSettings = Project.Scripts.Services.AudioServiceSystem.AudioSettings;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.Arena
{
    public class CompositionRoot : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private List<GameSystem.Arena> _arenaPrefabs;
        [SerializeField] private List<Enemy> _enemyPrefabs;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Ball _ballPrefab;

        [Header("UI Canvases")]
        [SerializeField] private StartGameCanvas _startGameCanvas;
        [SerializeField] private RankViewCanvas _rankViewCanvas;
        [SerializeField] private RewardCanvas _rewardCanvas;
        [SerializeField] private GameUICanvas _gameCanvas;
        [SerializeField] private TutorialCanvas _tutorialCanvas;
        [SerializeField] private UserInfoView _userInfoView;

        [Header("Services")]
        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private EffectsSetting _effectsSetting;
        [SerializeField] private Saves _saves;
        [SerializeField] private RewardButton _rewardButton;
        [SerializeField] private RewardService _rewardService;

        private GameFlowController _gameFlowController;
        private EntitySpawner _entitySpawner;
        private RewardController _rewardController;
        private RankHolder _rankHolder;

        private void Awake()
        {
            InitializeCoreSystems();
            InitializeUI();
            InitializeLocalization();
            InitializeGameFlow();
        }

        private void InitializeCoreSystems()
        {
            _rankHolder = new RankHolder();
            _rankHolder.Initialize();
            
            var effectHolder = new EffectHolder();
            var audioService = new AudioService(_audioSettings.GetData());
            
            _entitySpawner = new EntitySpawner(
                _playerPrefab, 
                _enemyPrefabs, 
                _ballPrefab, 
                _arenaPrefabs, 
                transform);
                
            _rewardController = new RewardController(
                _rewardService, 
                _rewardCanvas, 
                _rewardButton, 
                _startGameCanvas);
        }

        private void InitializeUI()
        {
            _rankViewCanvas.Initialize(_rankHolder);
            _userInfoView.Initialize(_rankHolder);
            _rewardCanvas.Initialize(_rewardService);
            _saves.Initialize(_rankHolder);
            _rewardController.Initialize();
        }

        private void InitializeLocalization()
        {
            LocalizationManager.Language = YG2.lang;
            YG2.SwitchLanguage(YG2.lang);
        }

        private void InitializeGameFlow()
        {
            _gameFlowController = new GameFlowController(
                _entitySpawner,
                _gameIn,
                _rewardController,
                _rankHolder,
                _gameCanvas,
                _startGameCanvas,
                _rankViewCanvas);
        }

        public void HandleFirstSession()
        {
            if (YG2.saves.ProgressData.IsFirstSession)
            {
                YG2.saves.ProgressData.IsFirstSession = false;
                YG2.SaveProgress();
                _tutorialCanvas.gameObject.SetActive(true);
            }
            else
            {
                _tutorialCanvas.gameObject.SetActive(false);
            }
        }

        private void OnEnable() => _gameFlowController.Enable();
        private void OnDisable() => _gameFlowController.Disable();
        private void Start() => _gameFlowController.StartGame();
    }


    public class GameInitializer
    {
        private readonly RankHolder _rankHolder;
        private readonly Saves _saves;
        private readonly StartGameCanvas _startGameCanvas;
        private readonly RankViewCanvas _rankViewCanvas;
        private readonly UserInfoView _userInfoView;
        private readonly RewardCanvas _rewardCanvas;
        private readonly GameUICanvas _gameCanvas;
        private readonly TutorialCanvas _tutorialCanvas;

        public GameInitializer(
            RankHolder rankHolder,
            Saves saves,
            StartGameCanvas startGameCanvas,
            RankViewCanvas rankViewCanvas,
            UserInfoView userInfoView,
            RewardCanvas rewardCanvas,
            GameUICanvas gameCanvas,
            TutorialCanvas tutorialCanvas)
        {
            _rankHolder = rankHolder;
            _saves = saves;
            _startGameCanvas = startGameCanvas;
            _rankViewCanvas = rankViewCanvas;
            _userInfoView = userInfoView;
            _rewardCanvas = rewardCanvas;
            _gameCanvas = gameCanvas;
            _tutorialCanvas = tutorialCanvas;
        }

        public void Initialize()
        {
            _rankHolder.Initialize();
            _rankViewCanvas.Initialize(_rankHolder);
            _userInfoView.Initialize(_rankHolder);
            _rewardCanvas.Initialize(_rewardService);
            _saves.Initialize(_rankHolder);
            
            LocalizationManager.Language = YG2.lang;
            YG2.SwitchLanguage(YG2.lang);
        }

        public void HandleFirstSession()
        {
            if (YG2.saves.ProgressData.IsFirstSession)
            {
                YG2.saves.ProgressData.IsFirstSession = false;
                YG2.SaveProgress();
                _tutorialCanvas.gameObject.SetActive(true);
            }
            else
            {
                _tutorialCanvas.gameObject.SetActive(false);
            }
        }
    }

    public class GameFlowController
    {
        private readonly EntitySpawner _entitySpawner;
        private readonly GameInitializer _gameInitializer;
        private readonly RewardController _rewardController;
        private readonly RankHolder _rankHolder;
        private readonly GameUICanvas _gameCanvas;
        private readonly StartGameCanvas _startGameCanvas;
        private readonly RankViewCanvas _rankViewCanvas;

        public GameFlowController(
            EntitySpawner entitySpawner,
            GameInitializer gameInitializer,
            RewardController rewardController,
            RankHolder rankHolder,
            GameUICanvas gameCanvas,
            StartGameCanvas startGameCanvas,
            RankViewCanvas rankViewCanvas)
        {
            _entitySpawner = entitySpawner;
            _gameInitializer = gameInitializer;
            _rewardController = rewardController;
            _rankHolder = rankHolder;
            _gameCanvas = gameCanvas;
            _startGameCanvas = startGameCanvas;
            _rankViewCanvas = rankViewCanvas;
        }

        public void Enable()
        {
            _startGameCanvas.StartGameButtonPressed += StartGame;
            _rankViewCanvas.RewardViewClosed += HandleRankCanvasClose;
            _rankHolder.RankRaised += _rewardController.SetRewardFlag;
        }

        public void Disable()
        {
            _startGameCanvas.StartGameButtonPressed -= StartGame;
            _rankViewCanvas.RewardViewClosed -= HandleRankCanvasClose;
            _rankHolder.RankRaised -= _rewardController.SetRewardFlag;
        }

        public void StartGame()
        {
            _gameInitializer.HandleFirstSession();
            _entitySpawner.SetupNewGame();
            _startGameCanvas.gameObject.SetActive(false);
            _gameCanvas.gameObject.SetActive(true);
            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());
        }

        private void HandleRankCanvasClose()
        {
            _entitySpawner.CleanupCurrentGame();
            _entitySpawner.SetupNewGame();
            _rankViewCanvas.gameObject.SetActive(false);
            _startGameCanvas.gameObject.SetActive(true);
        }

        public void HandleGameOver(int rankAmount)
        {
            _gameCanvas.gameObject.SetActive(false);
            _rankHolder.IncreaseRank(rankAmount);
            ProcessGameOver().Forget();
        }

        private async UniTaskVoid ProcessGameOver()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            await _rankViewCanvas.ShowResultsAsync();
            _rankViewCanvas.gameObject.SetActive(false);

            if (_rewardController.RewardRaised)
            {
                _rewardController.GiveReward();
            }
        }
    }

    public class EntitySpawner
    {
        private readonly Player _playerPrefab;
        private readonly List<Enemy> _enemyPrefabs;
        private readonly Ball _ballPrefab;
        private readonly List<GameSystem.Arena> _arenaPrefabs;
        private readonly Transform _parentTransform;

        private GameSystem.Arena _currentArena;
        private Ball _currentBall;
        private readonly List<Entity> _spawnedPlayers = new();
        private readonly List<List<Entity>> _spawnedEnemies = new();

        public EntitySpawner(
            Player playerPrefab,
            List<Enemy> enemyPrefabs,
            Ball ballPrefab,
            List<GameSystem.Arena> arenaPrefabs,
            Transform parentTransform)
        {
            _playerPrefab = playerPrefab;
            _enemyPrefabs = enemyPrefabs;
            _ballPrefab = ballPrefab;
            _arenaPrefabs = arenaPrefabs;
            _parentTransform = parentTransform;
        }

        public void SetupNewGame()
        {
            CreateNewArena();
            CreateBall();
            SetupSquads(_currentArena);
            _currentArena.GameOver += GameFlowController.HandleGameOver;
        }

        private void CreateNewArena()
        {
            if (_currentArena != null)
            {
                DestroyCurrentArena();
            }

            var arenaPrefab = _arenaPrefabs[Random.Range(0, _arenaPrefabs.Count)];
            _currentArena = Object.Instantiate(arenaPrefab, _parentTransform.position, Quaternion.identity);
        }

        private void CreateBall()
        {
            if (_currentBall != null)
            {
                DestroyCurrentBall();
            }

            const float ballOffsetX = -2.5f;
            const float ballOffsetY = 1f;
            const float ballOffsetZ = -1.5f;
            var ballPosition = new Vector3(
                _parentTransform.position.x + ballOffsetX,
                _parentTransform.position.y + ballOffsetY,
                _parentTransform.position.z + ballOffsetZ);

            _currentBall = Object.Instantiate(_ballPrefab, ballPosition, Quaternion.identity);
        }

        private void SetupSquads(GameSystem.Arena arena)
        {
            ClearEntities();
            
            for (int i = 0; i < arena.Squads.Count; i++)
            {
                if (i == 0)
                {
                    FillPlayerSquad(arena.Squads[i]);
                }
                else
                {
                    FillEnemySquad(arena.Squads[i]);
                }
            }
        }

        private void FillPlayerSquad(Squad squad)
        {
            _spawnedPlayers.Clear();
            
            for (int i = 0; i < squad.SpawnPoints.Count; i++)
            {
                var player = Object.Instantiate(_playerPrefab);
                player.transform.position = squad.SpawnPoints[i].position;
                _spawnedPlayers.Add(player);
            }

            InitializeSquad(_spawnedPlayers, squad);
        }

        private void FillEnemySquad(Squad squad)
        {
            var enemies = new List<Entity>();
            var enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
            
            for (int i = 0; i < squad.SpawnPoints.Count; i++)
            {
                var enemy = Object.Instantiate(enemyPrefab);
                enemy.transform.position = squad.SpawnPoints[i].position;
                enemies.Add(enemy);
            }

            _spawnedEnemies.Add(enemies);
            InitializeSquad(enemies, squad);
        }

        private void InitializeSquad(List<Entity> entities, Squad squad)
        {
            foreach (var entity in entities)
                entity.Initialize(squad.SquadZone, entities, _currentBall);

            squad.Initialize(entities);
        }

        public void CleanupCurrentGame()
        {
            ClearEntities();
            DestroyCurrentArena();
            DestroyCurrentBall();
        }

        private void ClearEntities()
        {
            foreach (var player in _spawnedPlayers)
                if (player != null) Object.Destroy(player.gameObject);
                
            foreach (var enemyGroup in _spawnedEnemies)
                foreach (var enemy in enemyGroup)
                    if (enemy != null) Object.Destroy(enemy.gameObject);

            _spawnedPlayers.Clear();
            _spawnedEnemies.Clear();
        }

        private void DestroyCurrentArena()
        {
            if (_currentArena != null)
                Object.Destroy(_currentArena.gameObject);
        }

        private void DestroyCurrentBall()
        {
            if (_currentBall != null)
                Object.Destroy(_currentBall.gameObject);
        }
    }

    public class RewardController
    {
        private readonly RewardService _rewardService;
        private readonly RewardCanvas _rewardCanvas;
        private readonly RewardButton _rewardButton;
        private readonly StartGameCanvas _startGameCanvas;

        public bool RewardRaised { get; private set; }

        public RewardController(
            RewardService rewardService,
            RewardCanvas rewardCanvas,
            RewardButton rewardButton,
            StartGameCanvas startGameCanvas)
        {
            _rewardService = rewardService;
            _rewardCanvas = rewardCanvas;
            _rewardButton = rewardButton;
            _startGameCanvas = startGameCanvas;
        }

        public void Initialize()
        {
            _rewardService.Initialize();
            _rewardButton.RewardButtonClicked += ShowReward;
            _rewardCanvas.RewardCanvasClosed += HideReward;
        }

        public void SetRewardFlag() => RewardRaised = true;

        public void GiveReward()
        {
            ShowReward();
            _startGameCanvas.gameObject.SetActive(false);
        }

        private void ShowReward()
        {
            string id = "coin";
            YG2.RewardedAdvShow(id, () => _rewardCanvas.gameObject.SetActive(true));
            RewardRaised = false;
        }

        private void HideReward()
        {
            _rewardCanvas.gameObject.SetActive(false);
            _startGameCanvas.gameObject.SetActive(true);
        }
    }
}