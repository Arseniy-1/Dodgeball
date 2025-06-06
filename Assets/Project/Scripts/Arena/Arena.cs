using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
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
    
    private int _deathCount = 0;

    public List<Squad> Squads => _squads;

    public event Action GameOver;

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
        
        _deathCount++;

        if (_deathCount == _squads.Count - 1)
            GameOver?.Invoke();
    }

    private void HandlePlayerSquadDeath(Squad squad)
    {
        squad.LostPlayers -= HandlePlayerSquadDeath;
        
        GameOver?.Invoke();
    }
    
    private async void EnableFrame()
    {
        while (isActiveAndEnabled)
        {
            await WaitForHitAsync();
            float delay = Random.Range(_minInactiveInterval, _maxInactiveInterval);
            await Task.Delay((int)(delay * 1000));
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
}