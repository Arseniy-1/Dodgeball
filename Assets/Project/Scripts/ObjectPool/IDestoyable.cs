using System;

namespace Project.Scripts.ObjectPool
{
    public interface IDestoyable<T>
    {
        public event Action<T> Destroyed;

        void Die();
    }
}