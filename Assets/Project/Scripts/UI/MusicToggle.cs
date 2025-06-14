using UnityEngine;
using UnityEngine.Audio;
using YG;

public class MusicToggle : SettingToggle
{
    [SerializeField] private AudioMixerGroup _audioMixer;
    
    private float _minlVolume = -80;
    private float _maxlVolume = 20;
    
    protected override bool IsEnabled()
    {
        return YandexGame.savesData.IsMusicEnabled;
    }

    protected override void EnableSetting()
    {
        float currentVolume;

        if (IsEnabled())
            currentVolume = _maxlVolume;
        else
            currentVolume = _minlVolume;
        
        _audioMixer.audioMixer.SetFloat(_audioMixer.name, Mathf.Log10(currentVolume) * 40);
        YandexGame.savesData.IsMusicEnabled = !YandexGame.savesData.IsMusicEnabled;
    }
}

public abstract class AudioToggle : SettingToggle
{
    protected override void EnableSetting()
    {
        
    }
}