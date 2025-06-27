using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [field: SerializeField] public AnimationsHolder AnimationsHolder { get; private set; } = new AnimationsHolder();
        
        [SerializeField] private DodgeAnimationsData _startDodgeAnimations;
        [SerializeField] private DeathAnimationsData _startDeathAnimations;
        [SerializeField] private PrepareAnimationsData _startPrepareAnimations;
        
        [field: SerializeField] public SettingsData SettingsData { get; private set; } = new SettingsData();
        [field: SerializeField] public ProgressData ProgressData { get; private set; } = new ProgressData();
        
        
        public void InitializeSaves()
        {
            AnimationsHolder.Initialize(_startDodgeAnimations, _startDeathAnimations, _startPrepareAnimations);   
        }
    }
}