using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

[Serializable]
public class AnimationsHolder
{
    [SerializeField] private List<Constans.DodgeAnimations> _dodgeAnimations = new List<Constans.DodgeAnimations>();
    [SerializeField] private List<Constans.ThrowAnimations> _throwAnimations = new List<Constans.ThrowAnimations>();
    [SerializeField] private List<Constans.SelebrateAnimations> _selebrateAnimations = new List<Constans.SelebrateAnimations>();
    [SerializeField] private List<Constans.DeathAnimations> _deathAnimations = new List<Constans.DeathAnimations>();
    [SerializeField] private List<Constans.PrepareAnimations> _prepareAnimations = new List<Constans.PrepareAnimations>();
    [SerializeField] private List<Constans.MoveAnimations> _moveAnimations = new List<Constans.MoveAnimations>();

    public List<int> DodgeAnimationsHash { get; private set; } = new List<int>();
    public List<int> ThrowAnimationsHash { get; private set; } = new List<int>();
    public List<int> SelebrateAnimationsHash { get; private set; } = new List<int>();
    public List<int> DeathAnimationsHash { get; private set; } = new List<int>();
    public List<int> PrepareAnimationsHash { get; private set; } = new List<int>();
    public List<int> MoveAnimationsHash { get; private set; } = new List<int>();

    public void Initialize(StartAnimationsData startAnimationsData)
    {
        Debug.Log("animations initialized");
        
        foreach (var animation in startAnimationsData.DodgeAnimations)
            AddDodgeAnimation(animation);

        foreach (var animation in startAnimationsData.ThrowAnimations)
            AddThrowAnimation(animation);
            
        foreach (var animation in startAnimationsData.SelebrateAnimations)
            AddSelebrateAnimation(animation);

        foreach (var animation in startAnimationsData.DeathAnimations)
            AddDeathAnimation(animation);
        
        foreach (var animation in startAnimationsData.PrepareAnimations)
            AddPrepareAnimation(animation);
            
        foreach (var animation in startAnimationsData.MoveAnimations)
            AddMoveAnimation(animation);
    }

    public void AddDodgeAnimation(Constans.DodgeAnimations animation)
    {
        if (_dodgeAnimations.Contains(animation))
            return;
        
        _dodgeAnimations.Add(animation);
        DodgeAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }

    public void AddThrowAnimation(Constans.ThrowAnimations animation)
    {
        if (_throwAnimations.Contains(animation))
            return;
        
        _throwAnimations.Add(animation);
        ThrowAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }

    public void AddSelebrateAnimation(Constans.SelebrateAnimations animation)
    {
        if (_selebrateAnimations.Contains(animation))
            return;
        
        _selebrateAnimations.Add(animation);
        SelebrateAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }

    public void AddDeathAnimation(Constans.DeathAnimations animation)
    {
        if (_deathAnimations.Contains(animation)) 
            return;
        
        _deathAnimations.Add(animation);
        DeathAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }
    
    public void AddPrepareAnimation(Constans.PrepareAnimations animation)
    {
        if (_prepareAnimations.Contains(animation))
            return;
        
        _prepareAnimations.Add(animation);
        PrepareAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }

    public void AddMoveAnimation(Constans.MoveAnimations animation)
    {
        if (_moveAnimations.Contains(animation)) 
            return;
        
        _moveAnimations.Add(animation);
        MoveAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        
        YG2.SaveProgress();
    }
}