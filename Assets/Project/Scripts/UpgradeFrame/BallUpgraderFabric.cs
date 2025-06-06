using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class BallUpgraderFabric : SerializedMonoBehaviour
{
    [OdinSerialize] private Dictionary<Type, BallUpgradeInfo> _ballUpgradeInfo;
    
    public List<BallUpgrader> Create()
    {
        List<BallUpgrader> ballUpgraders = new List<BallUpgrader>();

        ChargeBallUpgrader chargeBallUpgrader = new ChargeBallUpgrader(_ballUpgradeInfo[typeof(ChargeBallUpgrader)]);        
        ballUpgraders.Add(chargeBallUpgrader);
        
        MultipleBallUpgrader multipleBallUpgrader = new MultipleBallUpgrader(_ballUpgradeInfo[typeof(MultipleBallUpgrader)]);
        ballUpgraders.Add(multipleBallUpgrader);
        
        return ballUpgraders;
    } 
}