using UnityEngine;

namespace Project.Scripts.Services.Testing
{
    public class TimeScaler : MonoBehaviour
    {
        [Range(0, 2)]
        [SerializeField] private float _timeScale = 1f;

        private void Update()
        {
            Time.timeScale = _timeScale;
        }
    }
}