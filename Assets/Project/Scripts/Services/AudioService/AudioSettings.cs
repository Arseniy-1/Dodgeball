using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "AudioSystem/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    [SerializeField] private AudioData[] _audioData;

    public Dictionary<AudioID, AudioData> GetData()
    {
        var dictionary = new Dictionary<AudioID, AudioData>();

        foreach (var data in _audioData)
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
    public struct AudioData
    {
        [HideLabel]
        [HorizontalGroup]
        public AudioID _id;
        [HideLabel]
        [HorizontalGroup]
        public List<AudioClip> _clips;
        [Range(0f,1f)]
        public float _volume;
        [Range(0f,1f)]
        public float _minPitch;
        [Range(1f,2f)]
        public float _maxPitch;

        public AudioID ID => _id;
        public IReadOnlyList<AudioClip> Clips => _clips;
        public float Volume => _volume;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
    }
}