using System;
using System.Threading;
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

    public void PlayAnimation(int animationHash, CancellationToken cancellationToken)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _animationLength = stateInfo.length;

        LoopPlayAnimation(animationHash, cancellationToken).Forget();
    }

    private async UniTaskVoid LoopPlayAnimation(int animationHash, CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            _animator.Play(animationHash);

            await UniTask.Delay(TimeSpan.FromSeconds(_animationLength));
        }
    }
}