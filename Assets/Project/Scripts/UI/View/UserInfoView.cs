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

        [SerializeField] private RankAmountBar _rankAmountBar;
        [SerializeField] private RankAmountTextView _rankAmountTextView;

        private RankHolder _rankHolder;

        private void OnDestroy()
        {
            _rankHolder.RankRaised -= OnRankRaised;
        }

        public void Initialize(RankHolder rankHolder)
        {
            _rankHolder = rankHolder;

            OnRankRaised();

            _rankHolder.RankRaised += OnRankRaised;
            _rankAmountBar.Initialize(_rankHolder);
            _rankAmountTextView.Initialize(_rankHolder);
        
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
    }
}