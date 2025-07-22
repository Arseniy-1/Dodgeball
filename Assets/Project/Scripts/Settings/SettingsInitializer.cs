using System.Collections.Generic;
using Project.Scripts.UI;
using UnityEngine;

namespace Project.Scripts.Settings
{
    public class SettingsInitializer : MonoBehaviour
    {
        [SerializeField] private List<SettingToggle> _settingToggles;

        private void Start() 
        {
            foreach (var toggle in _settingToggles)
                toggle.Initialize();
        }
    }
}