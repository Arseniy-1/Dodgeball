using Assets.SimpleLocalization.Scripts;
using Project.Scripts.Rank;
using Project.Scripts.SavesSystem;
using Project.Scripts.SavesSystem.AnimationSO;
using Project.Scripts.Settings;

namespace YG
{
    public partial class SavesYG
    {
        public StartAnimationsData StartAnimationsData;

        public AnimationsHolder AnimationsHolder = new();

        public SettingsData SettingsData = new();
        public ProgressData ProgressData = new();

        public RankHolder RankHolder;

        public void InitializeStartSaves(StartAnimationsData startAnimationsData, RankHolder rankHolder)
        {
            RankHolder = rankHolder;
            StartAnimationsData = startAnimationsData;

            SetStartAnimations();

            YG2.SaveProgress();
        }

        public void ResetProgress()
        {
            AnimationsHolder.ResetAnimations();
            SetStartAnimations();
            ProgressData.IsFirstSession = true;
            RankHolder.Reset();
            ResetLanguage();

            YG2.SetLeaderboard("Leaderboard", ProgressData.CurrentRank);
            YG2.SaveProgress();
        }

        public void ResetLanguage()
        {
            LocalizationManager.Language = Languages.ru.ToString();
            YG2.lang = Languages.ru.ToString();
            YG2.SwitchLanguage(Languages.ru.ToString());
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
    }
}