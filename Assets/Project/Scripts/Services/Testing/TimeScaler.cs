using UnityEngine;

namespace Project.Scripts.Services.Testing
{
    public class TimeScaler : MonoBehaviour
    {
        [SerializeField, Range(0, 2)] private float _timeScale = 1f;

        private void Update()
        {
            Time.timeScale = _timeScale;
        }
    }
}