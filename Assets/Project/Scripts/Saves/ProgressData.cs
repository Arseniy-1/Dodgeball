using System;

namespace Project.Scripts.Saves
{
    [Serializable]
    public class ProgressData
    {
        public int CurrentRank = 0;
        public int CurrentRankAmount = 0;
        public int PreviousRankAmount = 0;
        public bool IsFirstSession = true;
    }
}