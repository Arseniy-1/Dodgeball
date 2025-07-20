using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public class BallUpgraderFabric : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<Type, BallUpgradeInfo> _ballUpgradeInfo;
    
        public List<BallUpgrade> Create()
        {
            List<BallUpgrade> ballUpgraders = new List<BallUpgrade>();

            ChargeBallUpgrade chargeBallUpgrade = new ChargeBallUpgrade(_ballUpgradeInfo[typeof(ChargeBallUpgrade)]);        
            ballUpgraders.Add(chargeBallUpgrade);
        
            ElectricBallUpgrade electricBallUpgrade = new ElectricBallUpgrade(_ballUpgradeInfo[typeof(ElectricBallUpgrade)]);
            ballUpgraders.Add(electricBallUpgrade);
        
            PoisonBallUpgrade poisonBallUpgrade = new PoisonBallUpgrade(_ballUpgradeInfo[typeof(PoisonBallUpgrade)]);
            ballUpgraders.Add(poisonBallUpgrade);
        
            return ballUpgraders;
        } 
    }
}