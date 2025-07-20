using UnityEngine;

namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    [CreateAssetMenu(fileName = "BallUpgrade", menuName = "BallUpgrade/BallUpgrade", order = 51)]
    public class BallUpgradeInfo : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Sprite BackgroundView { get; private set; }
    }
}