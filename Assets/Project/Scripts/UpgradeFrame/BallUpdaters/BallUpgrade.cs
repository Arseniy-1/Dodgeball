namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public abstract class BallUpgrade
    {
        protected BallUpgrade(BallUpgradeInfo ballUpgradeInfo)
        {
            BallUpgradeInfo = ballUpgradeInfo;
        }
        
        public BallUpgradeInfo BallUpgradeInfo { get; private set; }
    
        public abstract void UpgradeBall(Ball ball);
    }
}