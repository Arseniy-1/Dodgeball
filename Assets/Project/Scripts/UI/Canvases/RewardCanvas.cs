using System.Threading.Tasks;
using UnityEngine;
using YG;

public class RewardCanvas : InteractiveCanvas
{
    [SerializeField] private Reward _reward;
    [SerializeField] private ModelView _modelView;
    
    private RewardService _rewardService;

    public void Initialize(RewardService rewardService)
    {
        _rewardService = rewardService;
    }
    
    protected override async void HandleButtonClick()
    {
        await _reward.ShowRewardAnimation();
        _modelView.gameObject.SetActive(true);
        
        YG2.saves.AnimationsHolder.AddDeathAnimation();
    }
}

public class ModelView : MonoBehaviour
{
    
} 