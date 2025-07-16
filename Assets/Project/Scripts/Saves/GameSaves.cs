using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        public StartAnimationsData StartAnimationsData;

        public AnimationsHolder AnimationsHolder = new AnimationsHolder();

        public SettingsData SettingsData = new SettingsData();
        public ProgressData ProgressData = new ProgressData();

        public void InitializeStartSaves(StartAnimationsData startAnimationsData)
        {
            StartAnimationsData = startAnimationsData;

            foreach (var animation in StartAnimationsData.DodgeAnimations)
                YG2.saves.AnimationsHolder.AddDodgeAnimation(animation);

            foreach (var animation in StartAnimationsData.CelebrateAnimations)
                YG2.saves.AnimationsHolder.AddCelebrateAnimation(animation);

            foreach (var animation in StartAnimationsData.DeathAnimations)
                YG2.saves.AnimationsHolder.AddDeathAnimation(animation);

            foreach (var animation in StartAnimationsData.PrepareAnimations)
                YG2.saves.AnimationsHolder.AddPrepareAnimation(animation);

            YG2.SaveProgress();
        }

        public void ResetProgress()
        {
            AnimationsHolder.ResetAnimations();
            ProgressData.IsFirstSession = true;
            Debug.Log(ProgressData.IsFirstSession);
            YG2.SaveProgress();
        }
    }
}