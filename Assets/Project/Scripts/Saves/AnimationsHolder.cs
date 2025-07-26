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
        public List<DodgeAnimations> DodgeAnimations = new ();
        public List<CelebrateAnimations> CelebrateAnimations = new ();
        public List<DeathAnimations> DeathAnimations = new ();
        public List<PrepareAnimations> PrepareAnimations = new ();

        public List<int> DodgeAnimationsHash = new ();
        public List<int> CelebrateAnimationsHash = new ();
        public List<int> DeathAnimationsHash = new ();
        public List<int> PrepareAnimationsHash = new ();

        public void AddDodgeAnimation(DodgeAnimations animation)
        {
            if (DodgeAnimations.Contains(animation))
                return;
        
            DodgeAnimations.Add(animation);
            DodgeAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }

        public void AddCelebrateAnimation(CelebrateAnimations animation)
        {
            if (CelebrateAnimations.Contains(animation))
                return;
        
            CelebrateAnimations.Add(animation);
            CelebrateAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }

        public void AddDeathAnimation(DeathAnimations animation)
        {
            if (DeathAnimations.Contains(animation)) 
                return;
        
            DeathAnimations.Add(animation);
            DeathAnimationsHash.Add(Animator.StringToHash(animation.ToString()));
        }
    
        public void AddPrepareAnimation(PrepareAnimations animation)
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