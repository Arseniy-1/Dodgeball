using System.Collections.Generic;
using Project.Scripts.UpgradeFrame.BallUpdaters;

namespace Project.Scripts.GameSystem
{
    public class BallUpgradeHolder
    {
        public BallUpgradeHolder(BallUpgraderFabric fabric)
        {
            Upgraders = fabric.Create();
        }
     
        public List<BallUpgrade> Upgraders { get; private set; }
    }
}