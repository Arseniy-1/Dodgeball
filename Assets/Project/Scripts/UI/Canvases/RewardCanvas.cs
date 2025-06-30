using System.Threading;
using UnityEngine;

public class RewardCanvas : InteractiveCanvas
{
    [SerializeField] private BoxView _boxView;
    [SerializeField, Range(0, 5)] private float _boxAnimationDuration;
    [SerializeField, Range(0, 8)] private float _boxEndScale = 1.5f;
    [SerializeField, Range(0, 3)] private float _boxWhiteFadeDuration = 0.3f;

    [SerializeField] private ModelView _modelView;
    [SerializeField] private PlayerInputController _playerInputController;
    private RewardService _rewardService;

    private CancellationTokenSource _cancellationTokenSource;

    protected override void OnDisable()
    {
        base.OnDisable();

        _cancellationTokenSource.Cancel();
        _boxView.Disable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _boxView.Enable();
    }

    public void Initialize(RewardService rewardService)
    {
        _rewardService = rewardService;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async void HandleButtonClick()
    {
        DisableButton();
        
        await _boxView.ShowBoxAnimation(_boxAnimationDuration, _boxEndScale, _boxWhiteFadeDuration);
        _boxView.Disable();

        // DeathAnimationData deathAnimationData = _rewardService.GetRandomDeathAnimation();
        CelebrateAnimationData celebrateAnimationData = _rewardService.GetRandomCelebrateAnimation();
        // YG2.saves.AnimationsHolder.AddDeathAnimation(deathAnimationData.AnimationType);

        _modelView.gameObject.SetActive(true);
        int animationHash = Animator.StringToHash(celebrateAnimationData.AnimationType.ToString());

        _modelView.ShowReward(animationHash, celebrateAnimationData.Name);

        _playerInputController.ActionButtonCanceled += CloseWindow;
    }

    private void CloseWindow()
    {
        _playerInputController.ActionButtonCanceled -= CloseWindow;
        _modelView.Disable();
        gameObject.SetActive(false);
    }
}