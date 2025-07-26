using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Services.EffectService
{
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
    }
}