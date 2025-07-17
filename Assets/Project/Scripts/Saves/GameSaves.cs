using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        public StartAnimationsData StartAnimationsData;

        public AnimationsHolder AnimationsHolder = new AnimationsHolder();

        public SettingsData SettingsData = new SettingsData();
        public ProgressData ProgressData = new ProgressData();

        public RankHolder RankHolder;
        
        public void InitializeStartSaves(StartAnimationsData startAnimationsData, RankHolder rankHolder)
        {
            RankHolder = rankHolder;
            StartAnimationsData = startAnimationsData;

            SetStartAnimations();

            YG2.SaveProgress();
        }

        private void SetStartAnimations()
        {
            foreach (var animation in StartAnimationsData.DodgeAnimations)
                YG2.saves.AnimationsHolder.AddDodgeAnimation(animation);

            foreach (var animation in StartAnimationsData.CelebrateAnimations)
                YG2.saves.AnimationsHolder.AddCelebrateAnimation(animation);

            foreach (var animation in StartAnimationsData.DeathAnimations)
                YG2.saves.AnimationsHolder.AddDeathAnimation(animation);

            foreach (var animation in StartAnimationsData.PrepareAnimations)
                YG2.saves.AnimationsHolder.AddPrepareAnimation(animation);
        }

        public void ResetProgress()
        {
            AnimationsHolder.ResetAnimations();
            SetStartAnimations();
            ProgressData.IsFirstSession = true;
            RankHolder.Reset();
            
            YG2.SetLeaderboard("Leaderboard", ProgressData.CurrentRank);
            YG2.SaveProgress();
        }
    }
}