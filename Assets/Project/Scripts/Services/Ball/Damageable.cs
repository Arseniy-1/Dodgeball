using UnityEngine;

namespace Project.Scripts.Services.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float _damageMultiplier = 2f;
    
        public int Damage => (int)(_rigidbody.velocity.magnitude * _damageMultiplier);

        private Rigidbody _rigidbody;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}