using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}