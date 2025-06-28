using System.Threading;
using UnityEngine;

public class RewardCanvas : InteractiveCanvas
{
    [SerializeField] private BoxView _boxView;
    [SerializeField] private ModelView _modelView;
    [SerializeField, Range(0, 5)] private float _boxAnimationDuration;

    private RewardService _rewardService;

    private CancellationTokenSource _cancellationTokenSource;

    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
        _boxView.Disable();
    }

    public void Initialize(RewardService rewardService)
    {
        _rewardService = rewardService;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async void HandleButtonClick()
    {
        _boxView.Enable();
        await _boxView.ShowBoxAnimation(_boxAnimationDuration, _cancellationTokenSource.Token);
        _boxView.Disable();
        
        DeathAnimationData deathAnimationData = _rewardService.GetRandomDeathAnimation();
        CelebrateAnimationData celebrateAnimationData = _rewardService.GetRandomCelebrateAnimation();
        // YG2.saves.AnimationsHolder.AddDeathAnimation(deathAnimationData.AnimationType);

        _modelView.gameObject.SetActive(true);
        int animationHash = Animator.StringToHash(celebrateAnimationData.AnimationType.ToString());

        _modelView.ShowReward(animationHash, celebrateAnimationData.Name);
    }
}