using Project.Scripts.Services.CameraShakeService;

namespace Project.Scripts.Messages
{
    public struct M_CameraShake
    {
        public M_CameraShake(ShakeID shakeId)
        {
            ShakeID = shakeId;
        }

        public ShakeID ShakeID { get; private set; }
    }
}