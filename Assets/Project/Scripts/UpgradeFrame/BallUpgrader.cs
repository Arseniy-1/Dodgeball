public abstract class BallUpgrader
{
    public BallUpgradeInfo BallUpgradeInfo { get; private set; }

    public BallUpgrader(BallUpgradeInfo ballUpgradeInfo)
    {
        BallUpgradeInfo = ballUpgradeInfo;
    }
    
    public abstract void UpgradeBall(Ball ball);
}