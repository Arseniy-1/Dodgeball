using YG;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsToggle : AudioToggle
{
    protected override bool IsEnabled()
    {
        return YG2.saves.SettingsData.IsSoundsEnabled;
    }

    protected override void Toggle()
    {
        YG2.saves.SettingsData.IsSoundsEnabled = !YG2.saves.SettingsData.IsSoundsEnabled;
        EnableVolume();
    }
}