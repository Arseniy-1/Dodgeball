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
        float currentVolume;

        if (IsEnabled())
            currentVolume = _maxlVolume;
        else
            currentVolume = _minlVolume;
        
        _audioMixer.audioMixer.SetFloat(_audioMixer.name, Mathf.Log10(currentVolume) * 40);
    }
}