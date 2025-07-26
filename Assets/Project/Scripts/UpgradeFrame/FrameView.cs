using Project.Scripts.UpgradeFrame.BallUpdaters;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UpgradeFrame
{
    public class FrameView : MonoBehaviour
    {
        [SerializeField] private Image _backgroundView;
        [SerializeField] private Image _upgradeIcon;
        
        public void Initialize(BallUpgradeInfo ballUpgradeInfo)
        {
            _backgroundView.sprite = ballUpgradeInfo.BackgroundView;
            _upgradeIcon.sprite = ballUpgradeInfo.Icon;
        }
    }
}