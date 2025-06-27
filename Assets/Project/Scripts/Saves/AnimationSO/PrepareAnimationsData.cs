using UnityEngine;

[CreateAssetMenu(fileName = "PrepareAnimations", menuName = "Data/PrepareAnimations", order = 51)]
public class PrepareAnimationsData : ScriptableObject
{
    [field: SerializeField] public Constans.PrepareAnimations[] Animations { get; private set; }
}