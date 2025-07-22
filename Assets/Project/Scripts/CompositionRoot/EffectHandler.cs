using System;
using Project.Scripts.Services.AudioService;
using Project.Scripts.Services.EffectService;
using UnityEngine;
using AudioSettings = Project.Scripts.Services.AudioService.AudioSettings;

namespace Project.Scripts.CompositionRoot
{
    [Serializable]
    public class EffectHandler
    {
        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private EffectsSetting _effectsSetting;
        
        private EffectService _effectService;
        private AudioService _audioService;

        public void Initialize()
        {
            _effectService = new EffectService(_effectsSetting.GetData());
            _audioService = new AudioService(_audioSettings.GetData());
        }
    }
}