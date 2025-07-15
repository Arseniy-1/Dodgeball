using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using YG;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class AnimationsHolder
{
    public List<Constans.DodgeAnimations> DodgeAnimations  = new List<Constans.DodgeAnimations>();
    public List<Constans.CelebrateAnimations> CelebrateAnimations = new List<Constans.CelebrateAnimations>();
    public List<Constans.DeathAnimations> DeathAnimations = new List<Constans.DeathAnimations>();
    public List<Constans.PrepareAnimations> PrepareAnimations  = new List<Constans.PrepareAnimations>();

    public List<int> DodgeAnimationsHash  = new List<int>();
    public List<int> CelebrateAnimationsHash  = new List<int>();
    public List<int> DeathAnimationsHash  = new List<int>();
    public List<int> PrepareAnimationsHash  = new List<int>();

    public void AddDodgeAnimation(Constans.DodgeAnimations animation)
    {
        if (DodgeAnimations.Contains(animation))
            return;
        
        DodgeAnimations.Add(animation);
        DodgeAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }

    public void AddCelebrateAnimation(Constans.CelebrateAnimations animation)
    {
        if (CelebrateAnimations.Contains(animation))
            return;
        
        CelebrateAnimations.Add(animation);
        CelebrateAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }

    public void AddDeathAnimation(Constans.DeathAnimations animation)
    {
        if (DeathAnimations.Contains(animation)) 
            return;
        
        DeathAnimations.Add(animation);
        DeathAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }
    
    public void AddPrepareAnimation(Constans.PrepareAnimations animation)
    {
        if (PrepareAnimations.Contains(animation))
            return;

        PrepareAnimations.Add(animation);
        PrepareAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
    }

    public void ResetAnimations()
    {
        DodgeAnimations.Clear();
        CelebrateAnimations.Clear();
        DeathAnimations.Clear();
        PrepareAnimations.Clear();
        
        DodgeAnimationsHash.Clear();
        CelebrateAnimationsHash.Clear();
        DeathAnimationsHash.Clear();
        PrepareAnimationsHash.Clear();
    }
}