using System;
using UniRx;
using UnityEngine;

namespace Project.Scripts.ObjectPool.Effects
{
    [Serializable]
    public class EffectsSpawner : Spawner<Effect>
    {
        private Transform _parent;
        

        public EffectsSpawner(Effect effect, Transform parent) 
            : base(effect)
        {
            _parent = parent;
        }
        
        protected override Pool<Effect> CreatePool()
        {
            return new EffectsPool(Prefab, StartAmount, _parent);
        }
    }
}