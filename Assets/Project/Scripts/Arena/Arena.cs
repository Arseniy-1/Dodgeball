using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _squads;
    [SerializeField] private Transform _ballPosition;
    [SerializeField] private BallUpgraderFabric _ballUpgraderFabric;
    
    [SerializeField] private List<BallUpgrader> _ballUpgraders;
    [SerializeField] private List<Frame> _frames;
    
    [SerializeField] private float _minInactiveInterval;
    [SerializeField] private float _maxInactiveInterval;
    
    private List<Squad> _deathSquads;
    private CancellationTokenSource _cancellationTokenSource;

    public List<Squad> Squads => _squads;

    public event Action GameOver;

    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _deathSquads = new List<Squad>();
    }
    
    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }
    
    public void StartGame(Ball ball)
    {
        _ballUpgraders = _ballUpgraderFabric.Create();
        
        ball.transform.position = _ballPosition.position;
        
        foreach (var squad in _squads)
        {
            if (squad.SquadType == typeof(Player))
                squad.LostPlayers += HandlePlayerSquadDeath;
            else
                squad.LostPlayers += HandleEnemySquadDeath;
        }

        EnableFrame();
    }

    private void HandleEnemySquadDeath(Squad squad)
    {
        squad.LostPlayers -= HandleEnemySquadDeath;
        
        _deathSquads.Add(squad);

        if (_deathSquads.Count == _squads.Count - 1)
        {
            NotifyWinner();
            GameOver?.Invoke();
        }
    }

    private void HandlePlayerSquadDeath(Squad squad)
    {
        squad.LostPlayers -= HandlePlayerSquadDeath;

        NotifyWinner();
        GameOver?.Invoke();
    }
    
    private async UniTaskVoid EnableFrame()
    {
        while (_cancellationTokenSource.IsCancellationRequested == false)
        {
            await WaitForHitAsync();
            float delay = Random.Range(_minInactiveInterval, _maxInactiveInterval);
            await UniTask.Delay((int)(delay * 1000) , cancellationToken: _cancellationTokenSource.Token);
        }
    }

    private async Task WaitForHitAsync()
    {
        int randomFrameIndex = Random.Range(0, _frames.Count);
        Frame selectedFrame = _frames[randomFrameIndex];

        var tcs = new TaskCompletionSource<bool>();

        void Handler(Frame frame)
        {
            selectedFrame.OnFrameHitted -= Handler;
            tcs.SetResult(true);
        }

        selectedFrame.OnFrameHitted += Handler;
        selectedFrame.Activate(_ballUpgraders[Random.Range(0, _ballUpgraders.Count)]);

        await tcs.Task;
    }
    
    private void NotifyWinner()
    {
        var winners = _squads.Except(_deathSquads);

        foreach (var squad in winners)
        {
            squad.Selebrate();
        }
    }
}