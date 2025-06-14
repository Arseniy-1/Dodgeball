using System;
using UnityEngine;

public class Rank
{
    private const int MaxAmount = 100;
    
    private int _currentAmount;

    public event Action RankRaised;
    
    private void IncreaseRank()
    {
        _currentAmount += 1;

        if (_currentAmount >= MaxAmount)
        {
            _currentAmount = 0;
            RankRaised?.Invoke();
        }
    }
}

public class RankHolder
{
    public Rank Rank;
}