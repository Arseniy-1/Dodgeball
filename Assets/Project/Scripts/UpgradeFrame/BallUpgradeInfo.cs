using UnityEngine;

[CreateAssetMenu(fileName = "BallUpgrader", menuName = "BallUpgrader/BallUpgrader", order = 51)]
public class BallUpgradeInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Sprite BackgroundView { get; private set; }
}