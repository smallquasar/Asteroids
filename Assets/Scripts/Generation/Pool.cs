using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public class Pool<T> where T : ICanSetActive, ICanSetGameObject, new()
    {
        private GameObject _poolObject;
        private Transform _poolContainer;

        private List<T> _available = new List<T>();
        private List<T> _inUse = new List<T>();

        public Pool(GameObject prefab, Transform poolContainer, int initialCount)
        {
            _poolObject = prefab;
            _poolContainer = poolContainer;

            for (int i = 0; i < initialCount; i++)
            {
                T instance = Create();
                _available.Add(instance);
            }
        }

        public T Get()
        {
            T instance = default(T);

            if (_available.Count > 0)
            {
                instance = _available[0];
                _available.Remove(instance);
            }
            else
            {
                instance = Create();
            }

            _inUse.Add(instance);

            return instance;
        }

        public void ReturnToPool(T instance)
        {
            if (!_inUse.Remove(instance))
                return;

            instance.SetActive(false);
            _available.Add(instance);
        }

        private T Create()
        {
            GameObject obj = Object.Instantiate(_poolObject, _poolContainer);
            T instance = new T();
            instance.SetGameObject(obj);
            instance.SetActive(false);

            return instance;
        }
    }
}
