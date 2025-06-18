using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 0.01f;
    
    public int Damage => (int)(_rigidbody.velocity.magnitude * _damageMultiplier);

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}