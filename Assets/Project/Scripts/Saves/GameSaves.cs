using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [SerializeField] private StartAnimationsData _startAnimationsData;
        [field: SerializeField] public AnimationsHolder AnimationsHolder { get; private set; } = new AnimationsHolder();

        [field: SerializeField] public SettingsData SettingsData { get; private set; } = new SettingsData();
        [field: SerializeField] public ProgressData ProgressData { get; private set; } = new ProgressData();


        public void InitializeSaves()
        {
            if (YG2.isFirstGameSession || true)
            {
                foreach (var animation in _startAnimationsData.DodgeAnimations)
                    YG2.saves.AnimationsHolder.AddDodgeAnimation(animation);

                foreach (var animation in _startAnimationsData.CelebrateAnimations)
                    YG2.saves.AnimationsHolder.AddCelebrateAnimation(animation);

                foreach (var animation in _startAnimationsData.DeathAnimations)
                    YG2.saves.AnimationsHolder.AddDeathAnimation(animation);

                foreach (var animation in _startAnimationsData.PrepareAnimations)
                    YG2.saves.AnimationsHolder.AddPrepareAnimation(animation);
                
                YG2.SaveProgress();
            }
        }
    }
}