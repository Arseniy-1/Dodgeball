using System;
using UnityEngine;

namespace Project.Scripts.ObjectPool.Effects
{
    [Serializable]
    public class EffectsSpawner : Spawner<Effect>
    {
        public EffectsSpawner(Effect effect, Transform parent) 
            : base(effect)
        {
        }
        
    }
}