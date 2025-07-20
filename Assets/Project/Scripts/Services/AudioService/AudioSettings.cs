using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Services.AudioService
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "AudioSystem/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField] private AudioData[] _audioData;

        public Dictionary<AudioID, AudioData> GetData()
        {
            var dictionary = new Dictionary<AudioID, AudioData>();

            foreach (var data in _audioData)
            {
                if (dictionary.TryAdd(data.ID, data) == false)
                {
                    Debug.LogWarning($"Duplicate AudioID detected: {data.ID}");
                }
            }

            return dictionary;
        }
    
        [Serializable]
        public struct AudioData
        {
            [HideLabel]
            [HorizontalGroup]
            public AudioID ID;
            [HideLabel]
            [HorizontalGroup]
            public List<AudioClip> Clips;
            [Range(0f,1f)]
            public float Volume;
            [MinMaxSlider(0f, 2f)]
            public Vector2 PitchRange;
        }
    }
}