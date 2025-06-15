using YG;

public class MusicToggle : AudioToggle
{
    protected override bool IsEnabled()
    {
        return YandexGame.savesData.IsMusicEnabled;
    }

    protected override void Toggle()
    {
        YandexGame.savesData.IsMusicEnabled = !YandexGame.savesData.IsMusicEnabled;
        EnableVolume();
    }
}