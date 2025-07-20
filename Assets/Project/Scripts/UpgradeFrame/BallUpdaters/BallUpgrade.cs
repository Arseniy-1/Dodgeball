namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public abstract class BallUpgrade
    {
        public BallUpgradeInfo BallUpgradeInfo { get; private set; }

        protected BallUpgrade(BallUpgradeInfo ballUpgradeInfo)
        {
            BallUpgradeInfo = ballUpgradeInfo;
        }
    
        public abstract void UpgradeBall(Ball ball);
    }
}