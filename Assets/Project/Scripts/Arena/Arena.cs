using System.Collections.Generic;
using System;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _squads;

    private int _deathCount = 0;

    public List<Squad> Squads => _squads;

    public event Action GameOver;

    public void StartGame()
    {
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