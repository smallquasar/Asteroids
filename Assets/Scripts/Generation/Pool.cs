using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public class Pool<T> where T : ICanSetActive, ICanSetGameObject
    {
        private IPoolObjectCreator<T> _poolObjectCreator;
        private GameObject _poolObject;
        private Transform _poolContainer;

        private List<T> _available = new List<T>();
        private List<T> _inUse = new List<T>();

        private bool _canExpandPool;

        public Pool(IPoolObjectCreator<T> poolObjectCreator, GameObject prefab, Transform poolContainer, int initialCount, bool canExpandPool)
        {
            _poolObjectCreator = poolObjectCreator;
            _poolObject = prefab;
            _poolContainer = poolContainer;
            _canExpandPool = canExpandPool;

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
                _inUse.Add(instance);
            }
            else if (_canExpandPool)
            {
                instance = Create();
                _inUse.Add(instance);
            }            

            return instance;
        }

        public void ReturnToPool(T instance)
        {
            if (!_inUse.Remove(instance))
                return;

            instance.SetActive(false);
            _available.Add(instance);
        }

        //public void ReturnAllObjectsToPool()
        //{
        //    foreach (var instance in _inUse)
        //    {
        //        ReturnToPool(instance);
        //    }
        //}

        private T Create()
        {
            GameObject obj = Object.Instantiate(_poolObject, _poolContainer); //перенести в creator?
            T instance = _poolObjectCreator.Create();
            instance.SetGameObject(obj);
            instance.SetActive(false);

            return instance;
        }
    }
}
