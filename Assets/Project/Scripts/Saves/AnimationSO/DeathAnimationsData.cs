using UnityEngine;

[CreateAssetMenu(fileName = "DeathAnimations", menuName = "Data/DeathAnimations", order = 51)]
public class DeathAnimationsData : ScriptableObject
{
    [field: SerializeField] public Constans.DeathAnimations[] Animations { get; private set; }
}