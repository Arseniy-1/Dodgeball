using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class BaseSettings<TID, TData> : ScriptableObject 
        where TData : ISettingData<TID>
    {
        [SerializeField] 
        [OnValueChanged(nameof(OnValueChanged))]
        private TData[] _effectData;

        public Dictionary<TID, TData> GetData()
        {
            return _effectData.ToDictionary(d => d.ID, d => d);
        }

#if UNITY_EDITOR
        private void OnValueChanged()
        {
            var ids = new HashSet<TID>();
            
            foreach (var data in _effectData)
            {
                if (ids.Add(data.ID) == false)
                {
                    Debug.LogWarning($"Duplicate ID detected: {data.ID} in {name}", this);
                }
            }
        }
#endif
    }
}