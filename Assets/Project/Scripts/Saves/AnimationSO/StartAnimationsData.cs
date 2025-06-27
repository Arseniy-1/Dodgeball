using UnityEngine;

[CreateAssetMenu(fileName = "StartAnimations", menuName = "Data/StartAnimations", order = 51)]
public class StartAnimationsData : ScriptableObject
{
    [field: SerializeField] public Constans.DodgeAnimations[] DodgeAnimations { get; private set; }
    [field: SerializeField] public Constans.ThrowAnimations[] ThrowAnimations { get; private set; }
    [field: SerializeField] public Constans.SelebrateAnimations[] SelebrateAnimations { get; private set; }
    [field: SerializeField] public Constans.DeathAnimations[] DeathAnimations { get; private set; }
    [field: SerializeField] public Constans.PrepareAnimations[] PrepareAnimations { get; private set; }
    [field: SerializeField] public Constans.MoveAnimations[] MoveAnimations { get; private set; }
}