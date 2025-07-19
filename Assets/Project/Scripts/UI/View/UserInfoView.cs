using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class UserInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentRank;
    [SerializeField] private TextMeshProUGUI _userName;

    [SerializeField] private RankAmountBar _rankAmountBar;
    [SerializeField] private RankAmountTextView _rankAmountTextView;

    private RankHolder _rankHolder;

    private void OnDestroy()
    {
        _rankHolder.RankRaised -= HandleRankRaised;
    }

    public void Initialize(RankHolder rankHolder)
    {
        _rankHolder = rankHolder;

        HandleRankRaised();

        _rankHolder.RankRaised += HandleRankRaised;
        _rankAmountBar.Initialize(_rankHolder);
        _rankAmountTextView.Initialize(_rankHolder);

        if (YG2.infoYG.Authorization.authorized)
        {
            _userName.text = YG2.player.name;
        }
        else
        {
            switch (YG2.lang)
            {
                case nameof(Lanquages.en):
                    _userName.text = "unauthorized";   
                    break;
                
                case nameof(Lanquages.ru):
                    _userName.text = "не авторизован";   
                    break;
                
                case nameof(Lanquages.tr):
                    _userName.text = "Yetkili değil";   
                    break;
            }
        }
    }

    private void HandleRankRaised()
    {
        _currentRank.text = _rankHolder.CurrentRank.ToString();
    }
}