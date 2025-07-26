using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Services.AudioServiceSystem
{
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