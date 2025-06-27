using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    [SerializeField] private List<SettingToggle> _settingToggles;

    private void Start() 
    {
        foreach (var toggle in _settingToggles)
            toggle.Initialize();
    }
}