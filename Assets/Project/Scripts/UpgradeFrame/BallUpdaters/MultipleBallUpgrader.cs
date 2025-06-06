using UnityEngine;

public class MultipleBallUpgrader : BallUpgrader
{
    public MultipleBallUpgrader(BallUpgradeInfo ballUpgradeInfo) : base(ballUpgradeInfo)
    {
        
    }
    
    public override void UpgradeBall(Ball ball)
    {
        Debug.Log("Multiple Ball");
    }
}