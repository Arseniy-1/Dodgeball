using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.SavesSystem
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class AnimationsHolder
    {
        public List<DodgeAnimations> DodgeAnimations = new();
        public List<CelebrateAnimations> CelebrateAnimations = new();
        public List<DeathAnimations> DeathAnimations = new();
        public List<PrepareAnimations> PrepareAnimations = new();

        public List<int> DodgeAnimationsHash = new();
        public List<int> CelebrateAnimationsHash = new();
        public List<int> DeathAnimationsHash = new();
        public List<int> PrepareAnimationsHash = new();

        public void AddDodgeAnimation(DodgeAnimations animation)
        {
            AddAnimation(animation, DodgeAnimations, DodgeAnimationsHash);
        }

        public void AddCelebrateAnimation(CelebrateAnimations animation)
        {
            AddAnimation(animation, CelebrateAnimations, CelebrateAnimationsHash);
        }

        public void AddDeathAnimation(DeathAnimations animation)
        {
            AddAnimation(animation, DeathAnimations, DeathAnimationsHash);
        }

        public void AddPrepareAnimation(PrepareAnimations animation)
        {
            AddAnimation(animation, PrepareAnimations, PrepareAnimationsHash);
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

        private void AddAnimation<T>(T animation, List<T> animations, List<int> hashes) where T : Enum
        {
            if (animations.Contains(animation))
            {
                return;
            }

            animations.Add(animation);
            hashes.Add(Animator.StringToHash(animation.ToString()));
        }
    }
}