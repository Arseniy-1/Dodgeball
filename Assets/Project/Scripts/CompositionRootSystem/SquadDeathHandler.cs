using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.CompositionRootSystem
{
    public class SquadDeathHandler
    {
        private readonly List<Squad> _allSquads;
        private readonly List<Squad> _deadSquads = new ();
        
        private Action<bool> _gameOverCallback;

        public SquadDeathHandler(List<Squad> squads)
        {
            _allSquads = squads;
        }

        public void Initialize(Action<bool> gameOverCallback)
        {
            _gameOverCallback = gameOverCallback;
            
            foreach (var squad in _allSquads)
            {
                squad.LostPlayers += OnSquadDeath;
            }
        }

        private void OnSquadDeath(Squad squad)
        {
            squad.LostPlayers -= OnSquadDeath;
            _deadSquads.Add(squad);

            bool isPlayerWin = _deadSquads.Count == _allSquads.Count - 1;
            _gameOverCallback?.Invoke(isPlayerWin);
            NotifyWinners();
        }
        
        private void NotifyWinners()
        {
            var winners = _allSquads.Except(_deadSquads);

            foreach (var squad in winners)
            {
                squad.Celebrate();
            }
        }
    }
}