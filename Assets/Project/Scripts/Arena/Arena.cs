using System.Collections.Generic;
using System;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _enitySquads;

    public event Action GameOver;

    private void OnDestroy()
    {
        foreach (var squad in _enitySquads)
            squad.LostPlayers -= HandleLostPlayers;
    }
    
    public void StartGame()
    {
        foreach (var squad in _enitySquads)
        {
            squad.Initialize();
            squad.LostPlayers += HandleLostPlayers;
        }
    }

    private void HandleLostPlayers()
    {
        GameOver?.Invoke();
    }
}