    using Project.Scripts.Rank;
using TMPro;
using UnityEngine;
using YG;
using YG.LanguageLegacy;

namespace Project.Scripts.UI.View
{
    public class UserInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentRank;
        [SerializeField] private TextMeshProUGUI _userName;
        [SerializeField] private LanguageYG _languageTranslator;

        [SerializeField] private ViewBar _viewBar;
        [SerializeField] private TextView _textView;
        
        private RankHolder _rankHolder;
        
        private void OnDestroy()
        {
            _rankHolder.RankAmountChanged -= OnValueChanged;
            _rankHolder.RankRaised -= OnRankRaised;
        }

        public void Initialize(RankHolder rankHolder)
        {
            _rankHolder = rankHolder;
            _rankHolder.RankAmountChanged += OnValueChanged;
            OnValueChanged(_rankHolder.CurrentAmount, _rankHolder.MaxRankAmount);
            
            OnRankRaised();

            _rankHolder.RankRaised += OnRankRaised;
        
            if (YG2.player.auth)
            {
                _languageTranslator.enabled = false;
                _userName.text = YG2.player.name;
            }
        }

        private void OnRankRaised()
        {
            _currentRank.text = _rankHolder.CurrentRank.ToString();
        }
        
        private void OnValueChanged(int current, int max)
        {
            _viewBar.UpdateView(current, max);
            _textView.UpdateView(current, max);
        }
    }
}