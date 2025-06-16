using System;
using YG;

public class RankHolder
{
    private const int MaxAmount = 100;
    
    private int _currentRank;
    
    private int _currentAmount;
    private int _previousAmount;
    
    public int CurrentAmount => _currentAmount;
    public int PreviousAmount => _previousAmount;

    public void Initialize()
    {
        _currentAmount = YandexGame.savesData.CurrentRankAmount;
        _previousAmount = YandexGame.savesData.PreviousRankAmount;
    }
    
    public void IncreaseRank()
    {
        _previousAmount = _currentAmount;
        _currentAmount += 15;

        if (_currentAmount >= MaxAmount)
        {
            _currentAmount = 0;
            MessageBrokerHolder.GameActions.Publish(new M_GrantChest());
        }
        
        YandexGame.savesData.CurrentRankAmount = _currentAmount;
        YandexGame.savesData.PreviousRankAmount = _previousAmount;
        YandexGame.SaveProgress();
    }
}