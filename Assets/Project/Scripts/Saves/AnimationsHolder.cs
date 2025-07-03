using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationsHolder
{
    [SerializeField] private List<Constans.DodgeAnimations> _dodgeAnimations = new List<Constans.DodgeAnimations>();
    [SerializeField] private List<Constans.CelebrateAnimations> _selebrateAnimations = new List<Constans.CelebrateAnimations>();
    [SerializeField] private List<Constans.DeathAnimations> _deathAnimations = new List<Constans.DeathAnimations>();
    [SerializeField] private List<Constans.PrepareAnimations> _prepareAnimations = new List<Constans.PrepareAnimations>();

    [field: SerializeField] public List<int> DodgeAnimationsHash { get; private set; } = new List<int>();
    [field: SerializeField] public List<int> CelebrateAnimationsHash { get; private set; } = new List<int>();
    [field: SerializeField] public List<int> DeathAnimationsHash { get; private set; } = new List<int>();
    [field: SerializeField] public List<int> PrepareAnimationsHash { get; private set; } = new List<int>();

    public void AddDodgeAnimation(Constans.DodgeAnimations animation)
    {
        if (_dodgeAnimations.Contains(animation))
            return;
        
        _dodgeAnimations.Add(animation);
        DodgeAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }

    public void AddCelebrateAnimation(Constans.CelebrateAnimations animation)
    {
        if (_selebrateAnimations.Contains(animation))
            return;
        
        _selebrateAnimations.Add(animation);
        CelebrateAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }

    public void AddDeathAnimation(Constans.DeathAnimations animation)
    {
        if (_deathAnimations.Contains(animation)) 
            return;
        
        _deathAnimations.Add(animation);
        DeathAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }
    
    public void AddPrepareAnimation(Constans.PrepareAnimations animation)
    {
        if (_prepareAnimations.Contains(animation))
            return;

        _prepareAnimations.Add(animation);
        PrepareAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }
}