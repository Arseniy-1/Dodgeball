using System;
using System.Collections.Generic;
using Project.Scripts.ObjectPool.Effects;
using Sirenix.OdinInspector;

namespace Project.Scripts.Services.EffectServiceSystem
{
    [Serializable]
    public struct EffectData
    {
        [HideLabel]
        [HorizontalGroup] public EffectID ID;

        [HideLabel] 
        [HorizontalGroup] public List<Effect> Effects;
    }
}