using UnityEngine;

namespace Project.Scripts.SavesSystem.AnimationSO
{
    [CreateAssetMenu(fileName = "RewardAnimations", menuName = "Data/RewardAnimations")]
    public class RewardAnimations : ScriptableObject
    {
        [field: SerializeField] public DodgeAnimationData[] DodgeAnimations { get; private set; }
        [field: SerializeField] public CelebrateAnimationData[] CelebrateAnimations { get; private set; }
        [field: SerializeField] public DeathAnimationData[] DeathAnimations { get; private set; }
        [field: SerializeField] public PrepareAnimationData[] PrepareAnimations { get; private set; }
    }
}