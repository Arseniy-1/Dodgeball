using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = _camera.rotation;
    }
}