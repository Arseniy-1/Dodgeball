using YG;

namespace Project.Scripts.UI
{
    public class MusicToggle : AudioToggle
    {
        protected override bool IsEnabled()
        {
            return YG2.saves.SettingsData.IsMusicEnabled;
        }

        protected override void Toggle()
        {
            base.Toggle();
            YG2.saves.SettingsData.IsMusicEnabled = !YG2.saves.SettingsData.IsMusicEnabled;
            EnableVolume();
        }
    }
}