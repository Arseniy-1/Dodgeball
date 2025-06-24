using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class AnimatorController
{
    private Animator _animator;
    private CancellationTokenSource _cancellationTokenSource;

    private int _idle = Animator.StringToHash(Constans.Animation.Idle);
    private int _run = Animator.StringToHash(Constans.Animation.Run);
    private int _throw = Animator.StringToHash(Constans.Animation.Throw);
    private int _prepareToThrow = Animator.StringToHash(Constans.Animation.PrepareToThrow);
    private int _dodgeIdle = Animator.StringToHash(Constans.Animation.DodgeIdle);

    private List<int> _dodges = new List<int>
    {
        Animator.StringToHash(Constans.Animation.DodgeRight),
        Animator.StringToHash(Constans.Animation.DodgeLeft),
        Animator.StringToHash(Constans.Animation.DodgeBackflip),
    };
    
    private List<int> _prepares = new List<int>
    {
        Animator.StringToHash(Constans.Animation.PrepareToFightGolf),
        Animator.StringToHash(Constans.Animation.PrepareToFightActiveStance),
        Animator.StringToHash(Constans.Animation.PrepareToFightPassiveStance),
        Animator.StringToHash(Constans.Animation.PrepareToFightWarmingUp),
    };
    
    private List<int> _selebrates = new List<int>
    {
        Animator.StringToHash(Constans.Animation.SelebrateVictory),
        Animator.StringToHash(Constans.Animation.SelebrateTwistDance),
        Animator.StringToHash(Constans.Animation.SelebrateSillyDance),
        Animator.StringToHash(Constans.Animation.SelebrateShufflingDance),
        Animator.StringToHash(Constans.Animation.SelebrateHipHopDance),
    };
    
    private List<int> _death = new List<int>
    {
        Animator.StringToHash(Constans.Animation.DeathFall),
        Animator.StringToHash(Constans.Animation.DeathFallBack),
        Animator.StringToHash(Constans.Animation.DeathSlowlyFallBack),
    };

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

    public void Run() => _animator.Play(_run);

    public UniTask Attack() => PlayAndWait(_throw, _cancellationTokenSource.Token);

    public void PrepareAttack() => _animator.Play(_prepareToThrow);
    public void Idle() => _animator.Play(_idle);
    public void DodgeIdle() => _animator.Play(_dodgeIdle);

    public void Selebrate()
    {
        int randomSelebrate = _selebrates[Random.Range(0, _selebrates.Count)];

        _animator.Play(randomSelebrate);
    }
    
    public void PrepareToBattle()
    {
        int randomPrepare = _prepares[Random.Range(0, _prepares.Count)];

        _animator.Play(randomPrepare);
    }
    
    public UniTask Death()
    {
        int randomDeath = _death[Random.Range(0, _death.Count)];

        return PlayAndWait(randomDeath, _cancellationTokenSource.Token);
    }
    
    public UniTask Dodge()
    {
        int randomDodge = _dodges[Random.Range(0, _dodges.Count)];

        return PlayAndWait(randomDodge, _cancellationTokenSource.Token);
    }
    
    private async UniTask PlayAndWait(int animationHash, CancellationToken token)
    {
        _animator.Play(animationHash);
        await UniTask.Yield();

        if(token.IsCancellationRequested || _animator == null)
            return;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        int attempts = 0;

        while (token.IsCancellationRequested == false && stateInfo.shortNameHash != animationHash && attempts < 60)
        {
            await UniTask.Yield();
            
            if(token.IsCancellationRequested || _animator == null)
                return;

            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            attempts++;
        }

        while (token.IsCancellationRequested == false && stateInfo.normalizedTime < 1f && !_animator.IsInTransition(0))
        {
            await UniTask.Yield();
            
            if(token.IsCancellationRequested || _animator == null)
                return;
            
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}