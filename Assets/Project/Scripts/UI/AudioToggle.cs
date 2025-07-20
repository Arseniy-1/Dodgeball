using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.UI
{
    public abstract class AudioToggle : SettingToggle
    {
        [SerializeField] private AudioMixerGroup _audioMixer;
    
        private readonly float _minVolume = -80;
        private readonly float _maxVolume = 0;

        public override void Initialize()
        {
            base.Initialize();
            EnableVolume();
        }
    
        protected void EnableVolume()
        {
            float volumeDB = IsEnabled() ? _maxVolume : _minVolume;
            _audioMixer.audioMixer.SetFloat(_audioMixer.name, volumeDB);
        }
    }
}