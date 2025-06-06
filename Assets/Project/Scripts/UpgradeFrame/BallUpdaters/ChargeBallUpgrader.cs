using UnityEngine;

public class ChargeBallUpgrader : BallUpgrader
{
    public ChargeBallUpgrader(BallUpgradeInfo ballUpgradeInfo) : base(ballUpgradeInfo)
    {
        
    }
    
    public override void UpgradeBall(Ball ball)
    {
        Debug.Log("Charge Ball");
    }
}