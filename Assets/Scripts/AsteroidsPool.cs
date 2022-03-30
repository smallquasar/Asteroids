using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPool
{
    private GameObject _poolObject;
    private Transform _poolContainer;

    private List<GameObject> _available = new List<GameObject>();
    private List<GameObject> _inUse = new List<GameObject>();

    public AsteroidsPool(GameObject prefab, Transform poolContainer, int initialCount)
    {
        _poolObject = prefab;
        _poolContainer = poolContainer;

        for (int i = 0; i < initialCount; i++)
        {
            var instance = Create();
            _available.Add(instance);
        }
    }

    public GameObject Get()
    {
        GameObject instance = null;

        if (_available.Count > 0)
        {
            instance = _available[0];
            _available.Remove(instance);
            _inUse.Add(instance);
        }

        return instance;
    }

    public void ReturnToPool(GameObject instance)
    {
        if (!_inUse.Remove(instance))
            return;

        instance.SetActive(false);
        _available.Add(instance);
    }

    private GameObject Create()
    {
        var instance = Object.Instantiate(_poolObject, _poolContainer);
        instance.SetActive(false);

        return instance;
    }
}
