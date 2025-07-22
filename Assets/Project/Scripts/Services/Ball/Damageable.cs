using UnityEngine;

namespace Project.Scripts.Services.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float _damageMultiplier = 2f;
    
        private Rigidbody _rigidbody;

        public int Damage => (int)(_rigidbody.velocity.magnitude * _damageMultiplier);
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}