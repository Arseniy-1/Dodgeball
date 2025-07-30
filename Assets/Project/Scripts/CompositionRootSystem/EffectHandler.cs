using System;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectServiceSystem;
using UnityEngine;
using AudioSettings = Project.Scripts.Services.AudioServiceSystem.AudioSettings;

namespace Project.Scripts.CompositionRootSystem
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

        public void Dispose()
        {
            _effectService.Dispose();
            _audioService.Dispose();
        }
    }
}