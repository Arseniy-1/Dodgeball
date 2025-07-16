using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AnimationsViewCanvas : GameCanvas
{
    [SerializeField] private ExitButton _exitButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;

    [SerializeField] private RewardModel _rewardModel;

    [SerializeField] private TextMeshProUGUI _countView;
    
    private List<int> _animations;

    private int _currentAnimationIndex = 0;

    private CancellationTokenSource _cancellationTokenSource;

    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _currentAnimationIndex = 0;
        
        _exitButton.ExitButtonClicked += Disable;
        _nextButton.onClick.AddListener(HandleNextButtonClicked);
        _previousButton.onClick.AddListener(HandlePreviousButtonClicked);

        SetAnimations();
        _rewardModel.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
        _nextButton.onClick.RemoveListener(HandleNextButtonClicked);
        _previousButton.onClick.RemoveListener(HandlePreviousButtonClicked);
    }

    private void SetAnimations()
    {
        _animations = new List<int>();

        _animations.AddRange(YG2.saves.AnimationsHolder.DodgeAnimationsHash);
        _animations.AddRange(YG2.saves.AnimationsHolder.CelebrateAnimationsHash);
        _animations.AddRange(YG2.saves.AnimationsHolder.DeathAnimationsHash);
        _animations.AddRange(YG2.saves.AnimationsHolder.PrepareAnimationsHash);
        
        HandleClick();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        _cancellationTokenSource.Cancel();
    }

    private void HandleNextButtonClicked()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        _currentAnimationIndex++;

        if (_currentAnimationIndex >= _animations.Count)
        {
            _currentAnimationIndex = 0;
        }

        HandleClick();
    }

    private void HandlePreviousButtonClicked()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        _currentAnimationIndex--;

        if (_currentAnimationIndex < 0)
        {
            _currentAnimationIndex = _animations.Count - 1;
        }

        HandleClick();
    }

    private void HandleClick()
    {
        AudioID.UISoft.PlayOneShot();
        _countView.text = _currentAnimationIndex + 1 + " / " + _animations.Count;
        
        int animationHash = _animations[_currentAnimationIndex];
        _rewardModel.PlayAnimation(animationHash, _cancellationTokenSource.Token);
    }
}