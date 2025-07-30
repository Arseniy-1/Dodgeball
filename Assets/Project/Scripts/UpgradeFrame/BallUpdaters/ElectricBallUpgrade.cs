using Project.Scripts.Services.EffectServiceSystem;
using UnityEngine;

namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public class ElectricBallUpgrade : BallUpgrade
    {
        private const float ElectricForce = 20f;
    
        public ElectricBallUpgrade(BallUpgradeInfo ballUpgradeInfo) 
            : base(ballUpgradeInfo)
        {
        }

        public override void UpgradeBall(Ball ball)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            ball.GetComponent<Rigidbody>().AddForce(randomDirection * ElectricForce, ForceMode.Impulse);
        
            EffectID.ElectricExplosion.PlayEffect(ball.transform);
        }
    }
}