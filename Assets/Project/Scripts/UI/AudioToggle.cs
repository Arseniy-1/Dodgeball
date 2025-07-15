using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioToggle : SettingToggle
{
    [SerializeField] private AudioMixerGroup _audioMixer;
    
    private float _minlVolume = -80;
    private float _maxlVolume = 0;

    public override void Initialize()
    {
        base.Initialize();
        EnableVolume();
    }
    
    protected void EnableVolume()
    {
        float volumeDB = IsEnabled() ? _maxlVolume : _minlVolume;
        _audioMixer.audioMixer.SetFloat(_audioMixer.name, volumeDB);
    }
}