using System.Collections.Generic;
using UnityEngine;
using YG;

public class SettingsInitializer : MonoBehaviour
{
    [SerializeField] private List<SettingToggle> _settingToggles;

    private void Start()
    {
        YandexGame.LoadProgress();
        YandexGame.SwitchLanguage(YandexGame.savesData.language);
        
        foreach (var toggle in _settingToggles)
            toggle.Initialize();
    }
}