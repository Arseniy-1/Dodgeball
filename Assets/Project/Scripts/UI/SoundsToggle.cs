using YG;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsToggle : AudioToggle
{
    protected override bool IsEnabled()
    {
        return YandexGame.savesData.IsSoundsEnabled;
    }

    protected override void Toggle()
    {
        YandexGame.savesData.IsSoundsEnabled = !YandexGame.savesData.IsSoundsEnabled;
        EnableVolume();
    }
}