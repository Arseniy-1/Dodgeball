using System.Collections.Generic;
using System;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _squads;

    public List<Squad> Squads => _squads;
    
    public event Action GameOver;

    private void OnDestroy()
    {
        foreach (var squad in _squads)
            squad.LostPlayers -= HandleLostPlayers;
    }
    
    public void StartGame()
    {
        foreach (var squad in _squads)
        {
            squad.LostPlayers += HandleLostPlayers;
        }
    }

    private void HandleLostPlayers()
    {
        GameOver?.Invoke();
    }
}