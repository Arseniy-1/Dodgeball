using System;

namespace Project.Scripts
{
    public interface IDestoyable<T>
    {
        public event Action<T> Destroyed;

        void Die();
    }
}