using UnityEngine;

[CreateAssetMenu(fileName = "DodgeAnimations", menuName = "Data/DodgeAnimations", order = 51)]
public class DodgeAnimationsData : ScriptableObject
{
    [field: SerializeField] public Constans.DodgeAnimations[] Animations { get; private set; }
}