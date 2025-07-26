using System;
using System.Collections.Generic;
using Project.Scripts.Saves.AnimationSO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Reward
{
    [Serializable]
    public class RewardService
    {
        [SerializeField] private RewardAnimations _rewardAnimations;

        private List<DodgeAnimationData> _availableDodge = new();
        private List<CelebrateAnimationData> _availableCelebrate = new();
        private List<DeathAnimationData> _availableDeath = new();
        private List<PrepareAnimationData> _availablePrepare = new();

        public int DodgeAnimationCount => _availableDodge.Count;
        public int CelebrateAnimationCount => _availableCelebrate.Count;
        public int DeathAnimationCount => _availableDeath.Count;
        public int PrepareAnimationCount => _availablePrepare.Count;

        public void Initialize()
        {
            _availableDodge = new List<DodgeAnimationData>(_rewardAnimations.DodgeAnimations);
            _availableCelebrate = new List<CelebrateAnimationData>(_rewardAnimations.CelebrateAnimations);
            _availableDeath = new List<DeathAnimationData>(_rewardAnimations.DeathAnimations);
            _availablePrepare = new List<PrepareAnimationData>(_rewardAnimations.PrepareAnimations);
        }

        public DodgeAnimationData GetRandomDodge() => GetRandom(_availableDodge);
        public CelebrateAnimationData GetRandomCelebrate() => GetRandom(_availableCelebrate);
        public DeathAnimationData GetRandomDeath() => GetRandom(_availableDeath);
        public PrepareAnimationData GetRandomPrepare() => GetRandom(_availablePrepare);

        private T GetRandom<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            int index = Random.Range(0, list.Count);
            var item = list[index];
            list.RemoveAt(index);
            
            return item;
        }
    }
}