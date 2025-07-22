using System;
using UnityEngine;

namespace Project.Scripts.Services
{
    [Serializable]
    public class ModelRotator
    {
        [Range(-200, 200)]
        [SerializeField] private float _rotationSpeed;
    
        private float _currentRotation = 0;

        public void Update(Transform holder)
        {
            _currentRotation -= Time.deltaTime * _rotationSpeed;
            holder.transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
        }
    }
}