using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class RewardModel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ModelRotator _rotator;

    private float _animationLength;
    private int _animationHash;
    
    private void Update()
    {
        _rotator.Update(transform);
    }

    public async void PlayAnimation(int animationHash)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        _animationHash = animationHash;
        _animationLength = stateInfo.length;
        
        _animator.Play(animationHash);
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.Yield();
        _animator.Play(animationHash);
        
        LoopPlayAnimation(animationHash).Forget();
    }

    private async UniTaskVoid LoopPlayAnimation(int animationHash)
    {
        while (enabled)
        {
            _animator.Play(animationHash);

            Debug.Log(_animationLength);
            await UniTask.Delay(TimeSpan.FromSeconds(_animationLength));
        }
    }

    [Button]
    public void Play()
    {
        if (!_animator.HasState(0, _animationHash))
        {
            Debug.LogError($"Animation hash {_animationHash} not found in Animator!");
            return;
        }
        _animator.Play(_animationHash);
    }
}