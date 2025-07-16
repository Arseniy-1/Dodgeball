using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Chargeable))]
[RequireComponent(typeof(Damageable))]
public class Ball : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set;}
    public Chargeable Chargeable { get; private set;}
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Chargeable = GetComponent<Chargeable>();
    }
}