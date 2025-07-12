using System.Collections.Generic;

public static class Constans
{
    public static class EnemyNames
    {
        private static List<string> _names = new List<string>
        {
            "Antony_124",
            "John_being",
            "Darchi^_^",
            "YourAstro",
            "Killer_8",
            "Porshe911",
            "S1mple",
            "Y0urWay",
            "ILovU",
            "\\UWU//",
        };
        
        public static string GetRandomName()
        {
            return _names[UnityEngine.Random.Range(0, _names.Count)];
        }
    }

    public enum ConstantAnimations
    {
        Throw,
        PrepareToThrow,
        Idle,
        Run,
        DodgeIdle,
    }

    public enum DodgeAnimations
    {
        DodgeRight,
        DodgeLeft,
        DodgeBackflip,
    }

    public enum CelebrateAnimations
    {
        CelebrateVictory,
        CelebrateTwistDance,
        CelebrateSillyDance,
        CelebrateShufflingDance,
        CelebrateHipHopDance,
    }

    public enum DeathAnimations
    {
        DeathFall,
        DeathFallBack,
        DeathSlowlyFallBack
    }

    public enum PrepareAnimations
    {
        PrepareToFightGolf,
        PrepareToFightActiveStance,
        PrepareToFightPassiveStance,
        PrepareToFightWarmingUp,
    }
}