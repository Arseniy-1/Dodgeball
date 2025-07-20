using System;
using System.Collections.Generic;
using Project.Scripts.Saves.AnimationSO;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Project.Scripts.Reward
{
    [Serializable]
    public class RewardService
    {
        [SerializeField] private RewardAnimations _rewardAnimations;
    
        private List<DodgeAnimationData> _availableDodgeAnimations = new();
        private List<CelebrateAnimationData> _availableCelebrateAnimations = new();
        private List<DeathAnimationData> _availableDeathAnimations = new();
        private List<PrepareAnimationData> _availablePrepareAnimations = new();
    
        public int DodgeAnimationCount => _availableDodgeAnimations.Count;
        public int CelebrateAnimationCount => _availableCelebrateAnimations.Count;
        public int DeathAnimationCount => _availableDeathAnimations.Count;
        public int PrepareAnimationCount => _availablePrepareAnimations.Count;

        public void Initialize()
        {
            _availableDodgeAnimations.Clear();
            _availableCelebrateAnimations.Clear();
            _availableDeathAnimations.Clear();
            _availablePrepareAnimations.Clear();

            foreach (var animation in _rewardAnimations.DodgeAnimations)
            {
                int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
                if (YG2.saves.AnimationsHolder.DodgeAnimationsHash.Contains(animationHash) == false)
                {
                    _availableDodgeAnimations.Add(new DodgeAnimationData
                    {
                        AnimationType = animation.AnimationType,
                        Name = animation.Name
                    });
                }
            }

            foreach (var animation in _rewardAnimations.CelebrateAnimations)
            {
                int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
                if (YG2.saves.AnimationsHolder.CelebrateAnimationsHash.Contains(animationHash) == false)
                {
                    _availableCelebrateAnimations.Add(new CelebrateAnimationData
                    {
                        AnimationType = animation.AnimationType,
                        Name = animation.Name
                    });
                }
            }

            foreach (var animation in _rewardAnimations.DeathAnimations)
            {
                int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
                if (YG2.saves.AnimationsHolder.DeathAnimationsHash.Contains(animationHash) == false)
                {
                    _availableDeathAnimations.Add(new DeathAnimationData
                    {
                        AnimationType = animation.AnimationType,
                        Name = animation.Name
                    });
                }
            }

            foreach (var animation in _rewardAnimations.PrepareAnimations)
            {
                int animationHash = Animator.StringToHash(animation.AnimationType.ToString());
            
                if (YG2.saves.AnimationsHolder.PrepareAnimationsHash.Contains(animationHash) == false)
                {
                    _availablePrepareAnimations.Add(new PrepareAnimationData
                    {
                        AnimationType = animation.AnimationType,
                        Name = animation.Name
                    });
                }
            }
        }

        public DodgeAnimationData GetRandomDodgeAnimation()
        {
            int index = Random.Range(0, _availableDodgeAnimations.Count);
            var animation = _availableDodgeAnimations[index];
            _availableDodgeAnimations.RemoveAt(index);

            return animation;
        }

        public CelebrateAnimationData GetRandomCelebrateAnimation()
        {
            int index = Random.Range(0, _availableCelebrateAnimations.Count);
            var animation = _availableCelebrateAnimations[index];
            _availableCelebrateAnimations.RemoveAt(index);

            return animation;
        }

        public DeathAnimationData GetRandomDeathAnimation()
        {
            int index = Random.Range(0, _availableDeathAnimations.Count);
            var animation = _availableDeathAnimations[index];
            _availableDeathAnimations.RemoveAt(index);

            return animation;
        }

        public PrepareAnimationData GetRandomPrepareAnimation()
        {
            int index = Random.Range(0, _availablePrepareAnimations.Count);
            var animation = _availablePrepareAnimations[index];
            _availablePrepareAnimations.RemoveAt(index);

            return animation;
        }
    }
}