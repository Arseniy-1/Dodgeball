using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

[Serializable]
public class RewardService
{
    [SerializeField] private RewardAnimations _rewardAnimations;
    private List<AnimationInfo> _animations = new List<AnimationInfo>();

    public void Initialize()
    {
        _animations.Clear();

        foreach (var animation in _rewardAnimations.DodgeAnimations)
        {
            int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
            if (YG2.saves.AnimationsHolder.DodgeAnimationsHash.Contains(animationHash) == false)
            {
                _animations.Add(new AnimationInfo
                {
                    Hash = animationHash,
                    Name = animation.Name
                });
            }
        }

        foreach (var animation in _rewardAnimations.CelebrateAnimations)
        {
            int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
            if (YG2.saves.AnimationsHolder.CelebrateAnimationsHash.Contains(animationHash) == false)
            {
                _animations.Add(new AnimationInfo
                {
                    Hash = animationHash,
                    Name = animation.Name
                });
            }
        }

        foreach (var animation in _rewardAnimations.DeathAnimations)
        {
            int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
            if (YG2.saves.AnimationsHolder.DeathAnimationsHash.Contains(animationHash) == false)
            {
                _animations.Add(new AnimationInfo
                {
                    Hash = animationHash,
                    Name = animation.Name
                });
            }
        }

        foreach (var animation in _rewardAnimations.PrepareAnimations)
        {
            int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
            if (YG2.saves.AnimationsHolder.PrepareAnimationsHash.Contains(animationHash) == false)
            {
                _animations.Add(new AnimationInfo
                {
                    Hash = animationHash,
                    Name = animation.Name
                });
            }
        }
    }

    public AnimationInfo GetRandomAnimation()
    {
        return _animations[Random.Range(0, _animations.Count)];
    }
}