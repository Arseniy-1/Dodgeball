using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Random = UnityEngine.Random;


public class AnimatorController
{
    private Animator _animator;

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

    public AnimatorController(Animator animator) => _animator = animator;

    public void Run() => _animator.Play(_run);

    public Task Attack() => PlayAndWait(_throw);

    public void PrepareAttack() => _animator.Play(_prepareToThrow);
    public void Idle() => _animator.Play(_idle);
    public void DodgeIdle() => _animator.Play(_dodgeIdle);

    public void PrepareToBattle()
    {
        int randomPrepare = _prepares[Random.Range(0, _prepares.Count)];

        _animator.Play(randomPrepare);
    }
    
    public Task Dodge()
    {
        int randomDodge = _dodges[Random.Range(0, _dodges.Count)];

        return PlayAndWait(randomDodge);
    }
    
    private async Task PlayAndWait(int animationHash)
    {
        _animator.Play(animationHash);
        await Task.Yield();

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        int attempts = 0;
        
        while (stateInfo.shortNameHash != animationHash && attempts < 60)
        {
            await Task.Yield();
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            attempts++;
        }

        while (stateInfo.normalizedTime < 1f && !_animator.IsInTransition(0))
        {
            await Task.Yield();
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }
    }

}