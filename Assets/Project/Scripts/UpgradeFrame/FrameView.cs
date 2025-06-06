using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FrameView : MonoBehaviour
{
    [SerializeField] private Image _backgroundView;
    [SerializeField] private Image _upgradeIcon;

    private void Start()
    {
        //Метод для жизненного цикла юнити
    }

    public void Initialize(BallUpgradeInfo ballUpgradeInfo)
    {
        _backgroundView.sprite = ballUpgradeInfo.BackgroundView;
        _upgradeIcon.sprite = ballUpgradeInfo.Icon;
    }
}