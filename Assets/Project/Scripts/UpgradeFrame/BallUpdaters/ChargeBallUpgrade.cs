using Project.Scripts.Services.EffectService;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public class ChargeBallUpgrade : BallUpgrade
    {
        private const float VelocityMultiplier = 1.5f;
    
        private CompositeDisposable _compositeDisposable;
    
        public ChargeBallUpgrade(BallUpgradeInfo ballUpgradeInfo) 
            : base(ballUpgradeInfo)
        {
        }
    
        public override void UpgradeBall(Ball ball)
        {
            _compositeDisposable = new CompositeDisposable();
        
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();

            rigidbody.velocity *= VelocityMultiplier;
        
            ball.OnCollisionEnterAsObservable()
                .Subscribe(_ =>
                {
                    HandleHit(ball.transform);
                })
                .AddTo(_compositeDisposable);
        }

        private void HandleHit(Transform ballTransform)
        {
            EffectID.FireExplosion.PlayEffect(ballTransform);
        
            _compositeDisposable.Dispose();
        }
    }
}