using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using YG;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private List<Arena> _arenaPrefabs;
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Ball _ballPrefab;

    [SerializeField] private StartGameCanvas _startGameCanvas;
    [SerializeField] private RankViewCanvas _rankViewCanvas;
    [SerializeField] private RewardCanvas _rewardCanvas;
    [SerializeField] private GameUICanvas _gameCanvas;
    [SerializeField] private TutorialCanvas _tutorialCanvas;
    [SerializeField] private UserInfoView _userInfoView;

    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private EffectsSetting _effectsSetting;

    [SerializeField] private Saves _saves;

    [SerializeField] private RewardButton _rewardButton;
    [SerializeField] private RewardService _rewardService;

    private bool _rewardRaised = false;

    private EffectService _effectService;
    private AudioService _audioService;
    private RankHolder _rankHolder;

    private Ball _ballInstance;

    private PlayerSpawner _playerSpawner;
    private List<EnemySpawner> _enemySpawners = new();

    private Arena _arenaInstance;

    private void Awake()
    {
        _effectService = new EffectService(_effectsSetting.GetData());
        _audioService = new AudioService(_audioSettings.GetData());
        _rewardCanvas.Initialize(_rewardService);

        _rewardService.Initialize();
        _rankHolder = new RankHolder();
        _rankHolder.Initialize();
        _rankViewCanvas.Initialize(_rankHolder);

        _userInfoView.Initialize(_rankHolder);

        _playerSpawner = new PlayerSpawner(_playerPrefab);

        for (int i = 0; i < _enemyPrefabs.Count; i++)
        {
            EnemySpawner enemySpawner = new EnemySpawner(_enemyPrefabs[i]);
            _enemySpawners.Add(enemySpawner);
        }

        YG2.SwitchLanguage(YG2.lang);
        LocalizationManager.Language = YG2.lang;
    }

    private void OnEnable()
    {
        _startGameCanvas.OnStartGameButtonPressed += StartGame;
        _rankViewCanvas.OnRewardViewClosed += HandleRankCanvasClose;
        _rankHolder.RankRaised += HandleRankRaised;
        _rewardButton.RewardButtonClicked += ShowReward;
    }

    private void OnDisable()
    {
        _startGameCanvas.OnStartGameButtonPressed -= StartGame;
        _rankViewCanvas.OnRewardViewClosed -= HandleRankCanvasClose;
        _rankHolder.RankRaised -= HandleRankRaised;
        _rewardButton.RewardButtonClicked -= ShowReward;
    }

    private void Start()
    {
        _saves.Initialize();
        CreateMap();
        _startGameCanvas.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        _tutorialCanvas.gameObject.SetActive(false);
        
        if (YG2.saves.ProgressData.IsFirstSession)
        {
            YG2.saves.ProgressData.IsFirstSession = false;
            YG2.SaveProgress();
            
            _tutorialCanvas.gameObject.SetActive(true);
        }
        
        _startGameCanvas.gameObject.SetActive(false);
        _gameCanvas.gameObject.SetActive(true);
        _arenaInstance.Initialize(_ballInstance);
        GameStatusService.Instance.Initialize(_ballInstance);

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

        float ballOffsetY = 2f;
        float ballOffsetX = 2f;
        float ballOffsetZ = 2f;
        Vector3 ballPosition = new Vector3(transform.position.x + ballOffsetX, transform.position.y + ballOffsetY,
            transform.position.z + ballOffsetZ);
        _ballInstance = Instantiate(_ballPrefab, ballPosition, Quaternion.identity);

        int playersCount = 0;
        int enemiesCount = 0;

        for (int i = 0; i < _arenaInstance.Squads.Count; i++)
        {
            if (i == 0)
            {
                FillPlayerSquad(_playerSpawner, _arenaInstance.Squads[i]);
                playersCount += _arenaInstance.Squads[i].SpawnPoints.Count;
            }
            else
            {
                FillEnemySquad(_enemySpawners[Random.Range(0, _enemySpawners.Count)], _arenaInstance.Squads[i]);
                enemiesCount += _arenaInstance.Squads[i].SpawnPoints.Count;
            }
        }

        _arenaInstance.GameOver += HandleGameOverWrapper;
    }

    private void HandleGameOverWrapper(int rankAmount)
    {
        _gameCanvas.gameObject.SetActive(false);
        _rankHolder.IncreaseRank(rankAmount);
        HandleGameOver().Forget();
    }

    private async UniTaskVoid HandleGameOver()
    {
        _arenaInstance.GameOver -= HandleGameOverWrapper;
        MessageBrokerHolder.GameActions.Publish(new M_GameOver());

        float waitTime = 3f;
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

        _rankViewCanvas.gameObject.SetActive(true);
        await _rankViewCanvas.ShowResultsAsync();
        _rankViewCanvas.gameObject.SetActive(false);

        if (_rewardRaised)
        {
            GiveReward();
        }
    }

    private void ShowReward()
    {
        string id = "coin"; // Передача id требуется для внутренней работы плагина

        YG2.RewardedAdvShow(id, GiveReward);
    }

    private void GiveReward()
    {
        _startGameCanvas.gameObject.SetActive(false);
        _rewardCanvas.gameObject.SetActive(true);
        _rewardRaised = false;

        _rewardCanvas.RewardCanvasClosed += HandleRewardCanvasClosed;
    }

    private void HandleRewardCanvasClosed()
    {
        _rewardCanvas.RewardCanvasClosed -= HandleRewardCanvasClosed;
        _rewardCanvas.gameObject.SetActive(false);
        _startGameCanvas.gameObject.SetActive(true);
    }

    private void HandleRankCanvasClose()
    {
        ClearEntities();
        CreateMap();
        _rankViewCanvas.gameObject.SetActive(false);
        _startGameCanvas.gameObject.SetActive(true);
    }

    private void ClearEntities()
    {
        foreach (var enemySpawner in _enemySpawners)
            enemySpawner.DisableSpawned();

        _playerSpawner.DisableSpawned();
    }

    private void FillPlayerSquad(PlayerSpawner playerSpawner, Squad squad)
    {
        List<Entity> players = new List<Entity>();

        for (int i = 0; i < squad.SpawnPoints.Count; i++)
        {
            Player player = playerSpawner.Spawn();
            player.transform.position = squad.SpawnPoints[i].position;

            players.Add(player);
        }

        foreach (var player in players)
            player.Initialize(squad.SquadZone, players, _ballInstance);

        squad.Initialize(players);
    }

    private void FillEnemySquad(EnemySpawner enemySpawner, Squad squad)
    {
        List<Entity> enemies = new List<Entity>();

        for (int i = 0; i < squad.SpawnPoints.Count; i++)
        {
            Enemy enemy = enemySpawner.Spawn();
            enemy.transform.position = squad.SpawnPoints[i].position;

            enemies.Add(enemy);
        }

        foreach (var enemy in enemies)
            enemy.Initialize(squad.SquadZone, enemies, _ballInstance);

        squad.Initialize(enemies);
    }

    private void HandleRankRaised()
    {
        _rewardRaised = true;
    }
}