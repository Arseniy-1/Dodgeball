using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Messages;
using Project.Scripts.Services.EffectServiceSystem;

namespace Project.Scripts.GameSystem
{
    public class SquadDeathHandler
    {
        private readonly List<Squad> _allSquads;
        private readonly List<Squad> _deadSquads = new ();
        
        public SquadDeathHandler(List<Squad> squads)
        {
            _allSquads = squads;

            foreach (var squad in _allSquads)
                squad.LostPlayers += OnLostPlayer;
        }

        private void OnLostPlayer(Squad squad)
        {
            squad.LostPlayers -= OnLostPlayer;
            _deadSquads.Add(squad);

            bool isPlayerWin = _deadSquads.Count == _allSquads.Count - 1;
            
            NotifyWinners();
            HandleGameOver(isPlayerWin);
        }
        
        private void NotifyWinners()
        {
            var winners = _allSquads.Except(_deadSquads);

            foreach (var squad in winners)
            {
                squad.Celebrate();
            }
        }
        
        private void HandleGameOver(bool isWin)
        {
            if (isWin)
            {
                EffectID.Confetti.PlayEffect(_allSquads[0].transform);
            }
            
            MessageBrokerHolder.GameActions.Publish(new M_GameOver(isWin));
        }
    }
}