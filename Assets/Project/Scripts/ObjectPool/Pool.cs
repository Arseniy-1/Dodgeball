using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ObjectPool
{
    [Serializable]
    public abstract class Pool<T> where T : MonoBehaviour
    {
        private int _startAmount;

        protected T Prefab { get; private set; }
        protected Stack<T> Stack { get; private set; } = new ();

        protected Pool(T prefab, int startAmount)
        {
            Prefab = prefab;
            _startAmount = startAmount;
        }

        public void Release(T template)
        {
            template.gameObject.SetActive(false); 
            Stack.Push(template);
        }

        public T Get()
        {
            if (Stack.TryPop(out T template) == false)
            {
                Stack.Push(Create());
                template = Stack.Pop();
            }
        
            template.gameObject.SetActive(true);

            return template;
        }
        
        protected abstract T Create();
    }
}