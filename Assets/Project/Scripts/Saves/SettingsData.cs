using System;

[Serializable]
public class SettingsData
{
    public bool IsSoundsEnabled = true;
    public bool IsMusicEnabled = true;
    public bool IsCameraShakeEnabled = true;
    public Languages Language = Languages.ru;
}