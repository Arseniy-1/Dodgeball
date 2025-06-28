using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class RewardModel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ModelRotator _rotator;

    private float _animationLength;
    
    private void Update()
    {
        _rotator.Update(transform);
    }

    public void PlayAnimation(int animationHash)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _animationLength = stateInfo.length;
        
        LoopPlayAnimation(animationHash).Forget();
    }

    private async UniTaskVoid LoopPlayAnimation(int animationHash)
    {
        while (enabled)
        {
            _animator.Play(animationHash);

            await UniTask.Delay(TimeSpan.FromSeconds(_animationLength));
        }
    }
}