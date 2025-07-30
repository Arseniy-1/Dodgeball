using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.SavesSystem.AnimationSO
{
    [CreateAssetMenu(fileName = "StartAnimations", menuName = "Data/StartAnimations", order = 51)]
    public class StartAnimationsData : ScriptableObject
    {
        [field: SerializeField] public DodgeAnimations[] DodgeAnimations { get; private set; }
        [field: SerializeField] public CelebrateAnimations[] CelebrateAnimations { get; private set; }
        [field: SerializeField] public DeathAnimations[] DeathAnimations { get; private set; }
        [field: SerializeField] public PrepareAnimations[] PrepareAnimations { get; private set; }
    }
}