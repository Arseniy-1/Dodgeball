using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using YG;
using Random = UnityEngine.Random;

public class AnimatorController
{
    private Animator _animator;
    private CancellationTokenSource _cancellationTokenSource;

    private int _idle = Animator.StringToHash(Constans.ConstantAnimations.Idle.ToString());
    private int _run = Animator.StringToHash(Constans.ConstantAnimations.Run.ToString());

    private int _throw = Animator.StringToHash(Constans.ConstantAnimations.Throw.ToString());
    private int _prepareToThrow = Animator.StringToHash(Constans.ConstantAnimations.PrepareToThrow.ToString());

    private int _dodgeIdle = Animator.StringToHash(Constans.ConstantAnimations.DodgeIdle.ToString());
    

    public AnimatorController(Animator animator)
    {
        _animator = animator;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public UniTask Attack() => PlayAndWait(_throw, _cancellationTokenSource.Token);
    public void PrepareAttack() => _animator.Play(_prepareToThrow);

    public void Run() => _animator.Play(_run);
    public void Idle() => _animator.Play(_idle);
    public void DodgeIdle() => _animator.Play(_dodgeIdle);

    public void Selebrate()
    {
        var animationHashes = YG2.saves.AnimationsHolder.CelebrateAnimationsHash; 
        int randomSelebrate = animationHashes[Random.Range(0, animationHashes.Count)];

        _animator.Play(randomSelebrate);
    }

    public void PrepareToBattle()
    {
        var animationHashes = YG2.saves.AnimationsHolder.PrepareAnimationsHash; 
        int randomPrepare = animationHashes[Random.Range(0, animationHashes.Count)];

        _animator.Play(randomPrepare);
    }

    public UniTask Death()
    {
        var animationHashes = YG2.saves.AnimationsHolder.DeathAnimationsHash; 
        int randomDeath = animationHashes[Random.Range(0, animationHashes.Count)];

        return PlayAndWait(randomDeath, _cancellationTokenSource.Token);
    }

    public UniTask Dodge()
    {
        var animationHashes = YG2.saves.AnimationsHolder.DodgeAnimationsHash; 
        int randomDodge = animationHashes[Random.Range(0, animationHashes.Count)];

        return PlayAndWait(randomDodge, _cancellationTokenSource.Token);
    }

    private async UniTask PlayAndWait(int animationHash, CancellationToken token)
    {
        _animator.Play(animationHash);
        await UniTask.Yield();

        if (token.IsCancellationRequested || _animator == null)
            return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        int attempts = 0;

        while (token.IsCancellationRequested == false && stateInfo.shortNameHash != animationHash && attempts < 60)
        {
            await UniTask.Yield();

            if (token.IsCancellationRequested || _animator == null)
                return;

            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            attempts++;
        }

        while (token.IsCancellationRequested == false && stateInfo.normalizedTime < 1f && !_animator.IsInTransition(0))
        {
            await UniTask.Yield();

            if (token.IsCancellationRequested || _animator == null)
                return;

            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}