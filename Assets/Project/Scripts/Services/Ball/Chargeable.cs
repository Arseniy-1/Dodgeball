using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Chargeable : MonoBehaviour
{
    [SerializeField] private float _dangerousSpeed = 10f;
    
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    
    public event Action OnCharged;
    
    public bool IsCharged => _velocity.magnitude >= _dangerousSpeed;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;
        OnCharged?.Invoke();
    }
}