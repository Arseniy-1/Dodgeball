using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Saves.AnimationSO
{
    [CreateAssetMenu(fileName = "StartAnimations", menuName = "Data/StartAnimations", order = 51)]
    public class StartAnimationsData : ScriptableObject
    {
        [field: SerializeField] public Constants.DodgeAnimations[] DodgeAnimations { get; private set; }
        [field: SerializeField] public Constants.CelebrateAnimations[] CelebrateAnimations { get; private set; }
        [field: SerializeField] public Constants.DeathAnimations[] DeathAnimations { get; private set; }
        [field: SerializeField] public Constants.PrepareAnimations[] PrepareAnimations { get; private set; }
    }
}