public class RankAmountTextView : TextView
{
    private RankHolder _rankHolder;
    
    private void OnDestroy()
    {
        _rankHolder.RankAmoutChanged -= OnValueChanged;
    }

    public void Initialize(RankHolder rankHolder)
    {
        _rankHolder = rankHolder;
        _rankHolder.RankAmoutChanged += OnValueChanged;
        OnValueChanged(_rankHolder.CurrentAmount, _rankHolder.MaxRankAmount);
    }
}