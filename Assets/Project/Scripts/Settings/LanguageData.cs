using UnityEngine;

namespace Project.Scripts.Settings
{
    [CreateAssetMenu(fileName = "LanguageData", menuName = "Language Data", order = 51)]
    public class LanguageData : ScriptableObject
    {
        [field: SerializeField] public Languages Language { get; private set; }
        [field: SerializeField] public Sprite View { get; private set; }
    }
}