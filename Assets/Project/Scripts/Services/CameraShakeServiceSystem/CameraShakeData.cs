using System;
using UnityEngine;

namespace Project.Scripts.Services.CameraShakeServiceSystem
{
    [Serializable]
    public class CameraShakeData
    {
        [field: SerializeField] public ShakeID ShakeId { get; private set; }
        
        [field: Range(0, 5f)]
        [field: SerializeField] public float Duration { get; private set; }
        
        [field: Range(0, 1f)]
        [field: SerializeField] public float Strength { get; private set; }
        
        [field: Range(0, 20)]
        [field: SerializeField] public int Vibrato { get; private set; }

        [field: Range(0, 180f)]
        [field: SerializeField] public float Randomness { get; private set; }
    }
}