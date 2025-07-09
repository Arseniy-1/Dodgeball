using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using YG;

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

    protected override void OnEnable()
    {
        base.OnEnable();

        _boxView.Enable();
        AudioID.WaitForRewardClicked.PlayOneShot();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

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
        AudioID.RewardClicked.PlayOneShot();
        DisableButton();

        AudioID.RewardCharged.PlayOneShot();
        await _boxView.ShowBoxAnimation(_boxAnimationDuration, _boxEndScale, _boxWhiteFadeDuration);
        AudioID.RewardReleased.PlayOneShot();
        _boxView.Disable();

        var availableAnimations = new List<System.Func<(string name, string type)>>();

        if (_rewardService.DodgeAnimationCount > 0)
        {
            availableAnimations.Add(() =>
            {
                var anim = _rewardService.GetRandomDodgeAnimation();
                // YG2.saves.AnimationsHolder.AddDodgeAnimation(anim.AnimationType);
                return (anim.Name, anim.AnimationType.ToString());
            });
        }

        if (_rewardService.CelebrateAnimationCount > 0)
        {
            availableAnimations.Add(() =>
            {
                var anim = _rewardService.GetRandomCelebrateAnimation();
                // YG2.saves.AnimationsHolder.AddCelebrateAnimation(anim.AnimationType);
                return (anim.Name, anim.AnimationType.ToString());
            });
        }

        if (_rewardService.DeathAnimationCount > 0)
        {
            availableAnimations.Add(() =>
            {
                var anim = _rewardService.GetRandomDeathAnimation();
                // YG2.saves.AnimationsHolder.AddDeathAnimation(anim.AnimationType);
                return (anim.Name, anim.AnimationType.ToString());
            });
        }

        if (_rewardService.PrepareAnimationCount > 0)
        {
            availableAnimations.Add(() =>
            {
                var anim = _rewardService.GetRandomPrepareAnimation();
                // YG2.saves.AnimationsHolder.AddPrepareAnimation(anim.AnimationType);
                return (anim.Name, anim.AnimationType.ToString());
            });
        }

        if (availableAnimations.Count > 0)
        {
            var selectedAnimation = availableAnimations[Random.Range(0, availableAnimations.Count)]();

            _modelView.gameObject.SetActive(true);
            int animationHash = Animator.StringToHash(selectedAnimation.name);
            _modelView.ShowReward(animationHash, selectedAnimation.name);
        }

        _playerInputController.ActionButtonCanceled += CloseWindow;
    }

    private void CloseWindow()
    {
        AudioID.RewardCompleted.PlayOneShot();
        _playerInputController.ActionButtonCanceled -= CloseWindow;
        _modelView.Disable();
        gameObject.SetActive(false);
    }
}