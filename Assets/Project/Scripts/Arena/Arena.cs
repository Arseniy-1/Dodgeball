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
    
    [SerializeField] private List<Frame> _frames;
    
    [SerializeField] private float _minInactiveInterval;
    [SerializeField] private float _maxInactiveInterval;
    
    private List<BallUpgrader> _ballUpgraders;
    private List<Squad> _deathSquads;
    private CancellationTokenSource _cancellationTokenSource;

    private int _maxWinRankAmount = 40;
    private int _minWinRankAmount = 15;
    
    private int _maxLoseRankAmount = 10;
    private int _minLoseRankAmount = 3;
    
    public List<Squad> Squads => _squads;

    public event Action<int> GameOver;

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
            NotifyWinners();
            
            int rankAmount = Random.Range(_minWinRankAmount, _maxWinRankAmount);
            GameOver?.Invoke(rankAmount);
        }
    }

    private void HandlePlayerSquadDeath(Squad squad)
    {
        squad.LostPlayers -= HandlePlayerSquadDeath;

        NotifyWinners();
        
        int rankAmount = Random.Range(_minLoseRankAmount, _maxLoseRankAmount);
        GameOver?.Invoke(rankAmount);
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
    
    private void NotifyWinners()
    {
        var winners = _squads.Except(_deathSquads);

        foreach (var squad in winners)
        {
            squad.Celebrate();
        }
    }
}