using YG;

public class CameraShakeToggle : SettingToggle
{
    protected override bool IsEnabled()
    {
        return YG2.saves.SettingsData.IsCameraShakeEnabled;
    }

    protected override void Toggle()
    {
        YG2.saves.SettingsData.IsCameraShakeEnabled = !YG2.saves.SettingsData.IsCameraShakeEnabled;
    }
}