using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _dangerousSpeed = 15f;
    [SerializeField] private float _damageMultiplier = 1.5f;
    
    public bool IsCharged => Rigidbody.velocity.magnitude >= _dangerousSpeed;
    public int Damage => (int)(Rigidbody.velocity.magnitude * _damageMultiplier);
    
    public Rigidbody Rigidbody { get; private set;}
    
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}