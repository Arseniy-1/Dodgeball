using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "Language Data", order = 51)]
public class LanguageData : ScriptableObject
{
    [field: SerializeField] public Lanquages Lanquage { get; private set; }
    [field: SerializeField] public Sprite View { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
}