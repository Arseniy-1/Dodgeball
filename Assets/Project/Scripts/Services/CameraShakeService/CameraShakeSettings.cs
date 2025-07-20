using System.Linq;
using UnityEngine;

namespace Project.Scripts.Services.CameraShakeService
{
    [CreateAssetMenu(fileName = "CameraShakeSettings", menuName = "CameraShake/CameraShakeSettings", order = 51)]
    public class CameraShakeSettings : ScriptableObject
    {
        [SerializeField] private CameraShakeData[] _cameraShakeDatas;

        public bool TryGet(ShakeID shakeID, out CameraShakeData shakeData)
        {
            shakeData = _cameraShakeDatas.FirstOrDefault(data => data.ShakeId == shakeID);

            return shakeData != null;
        }
    }
}