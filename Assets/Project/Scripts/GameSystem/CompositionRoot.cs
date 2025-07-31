using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Project.Scripts.Entities;
using Project.Scripts.GameSystem;
using Project.Scripts.ObjectPool;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using Project.Scripts.SavesSystem;
using Project.Scripts.UpgradeFrame;
using UnityEngine;
using YG;

public class CompositionRoot : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Player _playerPrefab;
    
    [Header("Systems")]
    [SerializeField] private RewardService _rewardService;
    [SerializeField] private EffectHolder _effectHolder;
    [SerializeField] private Saves _saves;
    [SerializeField] private UIHandler _uiHandler;
    [SerializeField] private MatchManager _matchManager;
    
    [Header("Gameplay")]
    [SerializeField] private MapFactory _mapFactory;
    [SerializeField] private Transform _ballPosition;
    [SerializeField] private List<Frame> _frames;
    
    private RankHolder _rankHolder;
    private RewardSystem _rewardSystem;
    private GameStateManager _gameStateManager;

    private void Awake()
    {
        _rankHolder = new RankHolder();
        InitializeCoreServices();
        InitializeLocalization();
        InitializeGameSystems();
        PrepareGameWorld();
    }

    private void InitializeCoreServices()
    {
        _rewardService.Initialize();
        _rankHolder.Initialize();
        _effectHolder.Initialize();
        _saves.Initialize(_rankHolder);
        _uiHandler.Initialize(_rewardService, _rankHolder);
    }

    private void InitializeLocalization()
    {
        LocalizationManager.Language = YG2.lang;
        YG2.SwitchLanguage(YG2.lang);
    }

    private void InitializeGameSystems()
    {
        _rewardSystem = new RewardSystem(_rankHolder, _rewardService, _uiHandler);
        _rewardSystem.Initialize();
    }

    private List<Squad> CreateSquads()
    {
        return new List<Squad>(); 
    }

    private void PrepareGameWorld()
    {
        var entityCreator = new EntityCreator();
        var playerSpawner = new Spawner<Player>(_playerPrefab);
        var enemySpawners = new List<Spawner<Enemy>>();

        foreach (var enemyPrefab in _enemyPrefabs)
            enemySpawners.Add(new Spawner<Enemy>(enemyPrefab));

        _mapFactory.Initialize(entityCreator, playerSpawner, enemySpawners);
        _mapFactory.CreateMap();
        
        _matchManager.Initialize(_mapFactory.BallInstance);
    }

    private void OnDestroy()
    {
        _rewardSystem?.Dispose();
        _effectHolder?.Dispose();
    }
}