using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ElectroBallUpgrader : BallUpgrader
{
    private const float ElectroForce = 20f;
    
    public ElectroBallUpgrader(BallUpgradeInfo ballUpgradeInfo) : base(ballUpgradeInfo)
    {
    }

    public override void UpgradeBall(Ball ball)
    {
        Vector3 randomDirection = Random.onUnitSphere;
        ball.GetComponent<Rigidbody>().AddForce(randomDirection * ElectroForce, ForceMode.Impulse);
        
        EffectID.ElectroExplosion.PlayEffect(ball.transform);
    }
}