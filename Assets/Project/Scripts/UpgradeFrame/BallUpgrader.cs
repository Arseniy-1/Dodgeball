public abstract class BallUpgrader
{
    public BallUpgradeInfo BallUpgradeInfo { get; private set; }  
    
    public abstract void UpgradeBall(Ball ball);
}