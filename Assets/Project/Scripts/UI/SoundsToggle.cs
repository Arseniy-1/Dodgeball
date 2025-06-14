using YG;
using UnityEngine;
using UnityEngine.Audio;


public class SoundsToggle : SettingToggle
{
    [SerializeField] private AudioMixerGroup _audioMixer;
    
    protected override bool IsEnabled()
    {
        return YandexGame.savesData.IsSoundsEnabled;
    }

    protected override void EnableSetting()
    {
        YandexGame.savesData.IsSoundsEnabled = !YandexGame.savesData.IsSoundsEnabled;
    }
}