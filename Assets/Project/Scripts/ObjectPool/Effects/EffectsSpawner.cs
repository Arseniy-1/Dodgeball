using System;
using UniRx;
using UnityEngine;

namespace Project.Scripts.ObjectPool.Effects
{
    [Serializable]
    public class EffectsSpawner : Spawner<Effect>
    {
        private Transform _parent;
        
        protected CompositeDisposable CompositeDisposable;

        public EffectsSpawner(Effect effect, Transform parent) 
            : base(effect)
        {
            _parent = parent;

            CompositeDisposable = new CompositeDisposable();
        }
        
        public void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        protected override Pool<Effect> CreatePool()
        {
            return new EffectsPool(Prefab, StartAmount, _parent);
        }
    }
}