using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Services.AudioServiceSystem
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
    }
}