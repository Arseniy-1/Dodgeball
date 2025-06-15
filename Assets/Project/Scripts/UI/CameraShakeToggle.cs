using YG;

public class CameraShakeToggle : SettingToggle
{
    protected override bool IsEnabled()
    {
        return YandexGame.savesData.IsCameraShakeEnabled;
    }

    protected override void Toggle()
    {
        YandexGame.savesData.IsCameraShakeEnabled = !YandexGame.savesData.IsCameraShakeEnabled;
    }
}