using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FrameView : MonoBehaviour
{
    [SerializeField] private Sprite _backgroundView;
    [SerializeField] private Sprite _upgradeIcon;
    
    public void Initialize(BallUpgradeInfo ballUpgradeInfo)
    {
        _backgroundView = ballUpgradeInfo.BackgroundView;
        _upgradeIcon = ballUpgradeInfo.Icon;
    }
}