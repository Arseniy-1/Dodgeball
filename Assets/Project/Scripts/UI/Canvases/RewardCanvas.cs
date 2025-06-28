using UnityEngine;
using UnityEngine.Serialization;
using YG;

public class RewardCanvas : InteractiveCanvas
{
    [FormerlySerializedAs("_reward")] [SerializeField] private BoxView boxView;
    [SerializeField] private ModelView _modelView;
    
    private RewardService _rewardService;

    public void Initialize(RewardService rewardService)
    {
        _rewardService = rewardService;
    }
    
    protected override async void HandleButtonClick()
    {
        await boxView.ShowRewardAnimation();
        
        DeathAnimationData deathAnimationData = _rewardService.GetRandomDeathAnimation();
        YG2.saves.AnimationsHolder.AddDeathAnimation(deathAnimationData.AnimationType);
        
        _modelView.gameObject.SetActive(true);
        int animationHash = Animator.StringToHash(deathAnimationData.AnimationType.ToString());
        
        _modelView.ShowReward(animationHash, deathAnimationData.Name);
    }
}