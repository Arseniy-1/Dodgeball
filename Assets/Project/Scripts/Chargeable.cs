using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Chargeable : MonoBehaviour
{
    [SerializeField] private float _dangerousSpeed = 15f;
    
    public bool IsCharged => _velocity.magnitude >= _dangerousSpeed;

    public Rigidbody _rigidbody;
    private Vector3 _velocity;
    
    private void Update()
    {
        // Debug.Log(_rigidbody.velocity.magnitude + ">=" + _dangerousSpeed);
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;
    }
}