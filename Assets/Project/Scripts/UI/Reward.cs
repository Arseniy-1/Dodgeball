using System;
using Cysharp.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] private Image _rewardBoxImage;
    
    private float _animationDuration = 2.5f;
    
    public async UniTask ShowRewardAnimation()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration));
    }

    private void Update()
    {
        //StaticAnimation
    }
}