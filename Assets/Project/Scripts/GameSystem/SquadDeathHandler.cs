using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entities;
using Project.Scripts.Messages;
using Project.Scripts.Services.EffectServiceSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameSystem
{
    public class SquadDeathHandler
    {
        private readonly List<Squad> _allSquads;
        private readonly List<Squad> _deadSquads = new();

        public SquadDeathHandler(List<Squad> squads)
        {
            _allSquads = squads;
            
            foreach (var squad in _allSquads)
                squad.LostPlayers += OnLostPlayers;
        }

        private void OnLostPlayers(Squad squad)
        {
            squad.LostPlayers -= OnLostPlayers;
            _deadSquads.Add(squad);

            if (_deadSquads.Count != _allSquads.Count - 1)
                return;

            NotifyWinners();
        }

        private void NotifyWinners()
        {
            List<Squad> winners = _allSquads.Except(_deadSquads).ToList();
            bool isPlayerWin = winners[0].SquadType == typeof(Player);

            HandleGameOver(isPlayerWin, winners[0].transform);

            foreach (var squad in winners)
                squad.Celebrate();
        }

        private void HandleGameOver(bool isWin, Transform transform)
        {
            if (isWin)
                EffectID.Confetti.PlayEffect(transform);

            MessageBrokerHolder.GameActions.Publish(new M_GameOver(isWin));
        }
    }
}