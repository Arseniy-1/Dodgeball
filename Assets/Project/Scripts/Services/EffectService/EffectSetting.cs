using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsSetting", menuName = "EffectsSystem/EffectsSetting")]
public class EffectsSetting : ScriptableObject
{
    [SerializeField] private EffectData[] _effectsData;

    public Dictionary<EffectID, EffectData> GetData()
    {
        var dictionary = new Dictionary<EffectID, EffectData>();

        foreach (var data in _effectsData)
        {
            if (dictionary.ContainsKey(data.ID) == false)
            {
                dictionary.Add(data.ID, data);
            }
            else
            {
                Debug.LogWarning($"Duplicate AudioID detected: {data.ID}");
            }
        }

        return dictionary;
    }

    [Serializable]
    public struct EffectData
    {
        [HideLabel] 
        [HorizontalGroup] 
        public EffectID _id;

        [HideLabel] 
        [HorizontalGroup] 
        public Effect _effect;

        
        public EffectID ID => _id;
        public Effect Effect => _effect;
    }
}
