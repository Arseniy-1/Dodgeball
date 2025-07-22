using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Saves
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class AnimationsHolder
    {
        public List<Constants.DodgeAnimations> DodgeAnimations = new ();
        public List<Constants.CelebrateAnimations> CelebrateAnimations = new ();
        public List<Constants.DeathAnimations> DeathAnimations = new ();
        public List<Constants.PrepareAnimations> PrepareAnimations = new ();

        public List<int> DodgeAnimationsHash = new ();
        public List<int> CelebrateAnimationsHash = new ();
        public List<int> DeathAnimationsHash = new ();
        public List<int> PrepareAnimationsHash = new ();

        public void AddDodgeAnimation(Constants.DodgeAnimations animation)
        {
            if (DodgeAnimations.Contains(animation))
                return;
        
            DodgeAnimations.Add(animation);
            DodgeAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }

        public void AddCelebrateAnimation(Constants.CelebrateAnimations animation)
        {
            if (CelebrateAnimations.Contains(animation))
                return;
        
            CelebrateAnimations.Add(animation);
            CelebrateAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }

        public void AddDeathAnimation(Constants.DeathAnimations animation)
        {
            if (DeathAnimations.Contains(animation)) 
                return;
        
            DeathAnimations.Add(animation);
            DeathAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }
    
        public void AddPrepareAnimation(Constants.PrepareAnimations animation)
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
}