using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using YG;

[Serializable]
public class AnimationsHolder
{
    [SerializeField] private List<Constans.DodgeAnimations> _dodgeAnimations = new List<Constans.DodgeAnimations>();
    [SerializeField] private List<Constans.DeathAnimations> _deathAnimations = new List<Constans.DeathAnimations>();
    [SerializeField] private List<Constans.PrepareAnimations> _prepareAnimations = new List<Constans.PrepareAnimations>();

    public List<int> DodgeAnimationsHash { get; private set; } = new List<int>();
    public List<int> DeathAnimationsHash { get; private set; } = new List<int>();
    public List<int> PrepareAnimationsHash { get; private set; } = new List<int>();

    public void Initialize(DodgeAnimationsData startDodgeAnimation, DeathAnimationsData startDeathAnimation,
        PrepareAnimationsData startPrepareAnimation)
    {
        foreach (var dodgeAnimation in startDodgeAnimation.Animations)
            AddDodgeAnimation(dodgeAnimation);

        foreach (var deathAnimation in startDeathAnimation.Animations)
            AddDeathAnimation(deathAnimation);
        
        foreach (var prepareAnimation in startPrepareAnimation.Animations)
            AddPrepareAnimation(prepareAnimation);
    }

    public void AddDodgeAnimation(Constans.DodgeAnimations dodgeAnimation)
    {
        _dodgeAnimations.Add(dodgeAnimation);
        DodgeAnimationsHash.Add(Animator.StringToHash(dodgeAnimation.ToString()));

        YG2.SaveProgress();
    }

    public void AddDeathAnimation(Constans.DeathAnimations deathAnimation)
    {
        _deathAnimations.Add(deathAnimation);
        DeathAnimationsHash.Add(Animator.StringToHash(deathAnimation.ToString()));

        YG2.SaveProgress();
    }
    
    public void AddPrepareAnimation(Constans.PrepareAnimations prepareAnimations)
    {
        _prepareAnimations.Add(prepareAnimations);
        PrepareAnimationsHash.Add(Animator.StringToHash(prepareAnimations.ToString()));

        YG2.SaveProgress();
    }
}