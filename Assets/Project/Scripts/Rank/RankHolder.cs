using System;
using YG;

namespace Project.Scripts.Rank
{
    public class RankHolder
    {
        private int _currentAmount;
        private int _previousAmount;

        public event Action<int, int> RankAmountChanged;
        public event Action RankRaised;

        public int MaxRankAmount { get; private set; } = 100;
        public int CurrentRank { get; private set; }
        public int CurrentAmount => _currentAmount;
        public int PreviousAmount => _previousAmount;

        public void Initialize()
        {
            _currentAmount = YG2.saves.ProgressData.CurrentRankAmount;
            _previousAmount = YG2.saves.ProgressData.PreviousRankAmount;
            CurrentRank = YG2.saves.ProgressData.CurrentRank;
        }

        public void IncreaseRank(int amount)
        {
            if (_currentAmount >= MaxRankAmount)
                _currentAmount = 0;

            if (amount <= 0)
                return;

            _previousAmount = _currentAmount;
            _currentAmount += amount;

            if (_currentAmount >= MaxRankAmount)
            {
                _currentAmount = MaxRankAmount;
                CurrentRank++;

                YG2.SetLeaderboard("Leaderboard", CurrentRank);
                RankRaised?.Invoke();
            }

            RankAmountChanged?.Invoke(_currentAmount, MaxRankAmount);

            YG2.saves.ProgressData.CurrentRankAmount = _currentAmount;
            YG2.saves.ProgressData.PreviousRankAmount = _previousAmount;
            YG2.saves.ProgressData.CurrentRank = CurrentRank;
            YG2.SaveProgress();
        }

        public void Reset()
        {
            _currentAmount = 0;
            _previousAmount = 0;
            CurrentRank = 0;
        
            YG2.saves.ProgressData.CurrentRankAmount = 0;
            YG2.saves.ProgressData.PreviousRankAmount = 0;
            YG2.saves.ProgressData.CurrentRank = 0;
        
            RankRaised?.Invoke();
            RankAmountChanged?.Invoke(_currentAmount, MaxRankAmount);
        }
    }
}