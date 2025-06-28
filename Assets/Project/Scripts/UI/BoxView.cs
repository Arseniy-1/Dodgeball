using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BoxView : MonoBehaviour
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