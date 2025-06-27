using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [field: SerializeField] public AnimationsHolder AnimationsHolder { get; private set; } = new AnimationsHolder();
        [SerializeField] private StartAnimationsData _startAnimationsData;
        
        [field: SerializeField] public SettingsData SettingsData { get; private set; } = new SettingsData();
        [field: SerializeField] public ProgressData ProgressData { get; private set; } = new ProgressData();
        
        
        public void InitializeSaves()
        {
            if(YG2.isFirstGameSession || true)
                AnimationsHolder.Initialize(_startAnimationsData);   
        }
    }
}