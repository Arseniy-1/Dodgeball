using Project.Scripts.Rank;

namespace Project.Scripts.UI.View
{
    public class RankAmountBar : ViewBar
    {
        private RankHolder _rankHolder;
    
        private void OnDestroy()
        {
            _rankHolder.RankAmountChanged -= OnValueChanged;
        }

        public void Initialize(RankHolder rankHolder)
        {
            _rankHolder = rankHolder;
            _rankHolder.RankAmountChanged += OnValueChanged;
            OnValueChanged(_rankHolder.CurrentAmount, _rankHolder.MaxRankAmount);
        }
    }
}