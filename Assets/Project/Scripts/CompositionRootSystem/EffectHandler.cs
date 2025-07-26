using System;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectService;
using UnityEngine;
using AudioSettings = Project.Scripts.Services.AudioServiceSystem.AudioSettings;

namespace Project.Scripts.CompositionRootSystem
{
    [Serializable]
    public class EffectHandler
    {
        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private EffectsSetting _effectsSetting;
        
        public void Initialize()
        {
            new EffectService(_effectsSetting.GetData());
            new AudioService(_audioSettings.GetData());
        }
    }
}