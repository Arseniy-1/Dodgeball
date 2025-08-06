using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Services.AudioServiceSystem
{
    [Serializable]
    public struct AudioData : ISettingData<AudioID>
    {
        [field: SerializeField]
        [field: HideLabel]
        [field: HorizontalGroup] public AudioID ID { get; private set; }
       
        [field: SerializeField]
        [field: HideLabel]
        [field: HorizontalGroup] public List<AudioClip> Clips { get; private set; }
        
        [field: SerializeField]
        [field: Range(0f,1f)] public float Volume { get; private set; }
        
        [field: SerializeField]
        [field: MinMaxSlider(0f, 2f)] public Vector2 PitchRange { get; private set; }
    }
}