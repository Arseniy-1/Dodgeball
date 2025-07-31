using System;
using System.Collections.Generic;
using Project.Scripts.ObjectPool.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Services.EffectServiceSystem
{
    [Serializable]
    public struct EffectData
    {
        [field: SerializeField]
        [field: HideLabel]
        [field: HorizontalGroup] public EffectID ID { get; private set; }

        [field: SerializeField]
        [field: HideLabel] 
        [field: HorizontalGroup] public List<Effect> Effects { get; private set; }
    }
}