using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationsHolder
{
    [field: SerializeField] public List<Constans.DodgeAnimations> _dodgeAnimations {get; private set;} = new List<Constans.DodgeAnimations>();
    [field: SerializeField] public List<Constans.CelebrateAnimations> _selebrateAnimations {get; private set;} = new List<Constans.CelebrateAnimations>();
    [field: SerializeField] public List<Constans.DeathAnimations> _deathAnimations {get; private set;} = new List<Constans.DeathAnimations>();
    [field: SerializeField] public List<Constans.PrepareAnimations> _prepareAnimations {get; private set;} = new List<Constans.PrepareAnimations>();

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