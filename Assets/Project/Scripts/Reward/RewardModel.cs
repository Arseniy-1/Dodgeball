using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class RewardModel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ModelRotator _rotator;

    private void Update()
    {
        _rotator.Update(transform);
    }

    public void PlayAnimation(int animationHash, CancellationToken cancellationToken)
    {
        LoopPlayAnimation(animationHash, cancellationToken).Forget();
    }

    private async UniTaskVoid LoopPlayAnimation(int animationHash, CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            _animator.Play(animationHash, 0, 0f); 
        
            await UniTask.NextFrame(cancellationToken: cancellationToken);
        
            float length = _animator.GetCurrentAnimatorStateInfo(0).length;
            
            await UniTask.Delay(TimeSpan.FromSeconds(length), 
                DelayType.Realtime, 
                cancellationToken: cancellationToken);
        }
    }
}