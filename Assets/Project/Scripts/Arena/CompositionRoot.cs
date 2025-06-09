using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private List<Arena> _arenaPrefabs;
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private StartGameCanvas _startGameCanvas;
    
    private Ball _ballInstance;

    private PlayerSpawner _playerSpawner;
    private List<EnemySpawner> _enemySpawners = new();

    private Arena _arenaInstance;

    private void Awake()
    {
        _playerSpawner = new PlayerSpawner(_playerPrefab);

        for (int i = 0; i < _enemyPrefabs.Count; i++)
        {
            EnemySpawner enemySpawner = new EnemySpawner(_enemyPrefabs[i]);
            _enemySpawners.Add(enemySpawner);
        }
    }

    private void OnEnable()
    {
        _startGameCanvas.OnStartGameButtonPressed += StartGame;
    }

    private void OnDisable()
    {
        _startGameCanvas.OnStartGameButtonPressed -= StartGame;
    }

    private void Start()
    {
        CreateMap();
        _startGameCanvas.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        _startGameCanvas.gameObject.SetActive(false);
        
        if(_ballInstance != null)
            Destroy(_ballInstance.gameObject);
        
        _ballInstance = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        
        _arenaInstance.StartGame(_ballInstance);
        
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

        for (int i = 0; i < _arenaInstance.Squads.Count; i++)
        {
            if (i == 0)
                FillPlayerSquad(_playerSpawner, _arenaInstance.Squads[i]);
            else
                FillEnemySquad(_enemySpawners[Random.Range(0, _enemySpawners.Count)], _arenaInstance.Squads[i]);
        }

        _arenaInstance.GameOver += HandleGameOver;
    }

    private void HandleGameOver()
    {
        _arenaInstance.GameOver -= HandleGameOver;

        ClearEntities();
        
        CreateMap();
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
            player.Initialize(squad.SquadZone, players, _ballInstance);
            player.transform.position = squad.SpawnPoints[i].position;

            players.Add(player);
        }
        
        squad.Initialize(players);
    }

    private void FillEnemySquad(EnemySpawner enemySpawner, Squad squad)
    {
        List<Entity> enemys = new List<Entity>();

        for (int i = 0; i < squad.SpawnPoints.Count; i++)
        {
            Enemy enemy = enemySpawner.Spawn();
            enemy.Initialize(squad.SquadZone, enemys, _ballInstance);
            enemy.transform.position = squad.SpawnPoints[i].position;

            enemys.Add(enemy);
        }

        squad.Initialize(enemys);
    }
}