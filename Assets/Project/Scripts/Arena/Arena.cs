using System.Collections.Generic;
using System;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _squads;
    [SerializeField] private Transform _ballPosition;
    
    [SerializeField] private List<Frame> _frames;
    
    private int _deathCount = 0;

    public List<Squad> Squads => _squads;

    public event Action GameOver;

    public void StartGame(Ball ball)
    {
        ball.transform.position = _ballPosition.position;
        
        foreach (var squad in _squads)
        {
            if (squad.SquadType == typeof(Player))
                squad.LostPlayers += HandlePlayerSquadDeath;
            else
                squad.LostPlayers += HandleEnemySquadDeath;
        }
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
    
    
}