using Assets.Scripts.Asteroids;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPool
{
    private GameObject _poolObject;
    private Transform _poolContainer;

    private List<AsteroidController> _available = new List<AsteroidController>();
    private List<AsteroidController> _inUse = new List<AsteroidController>();

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

    public AsteroidController Get()
    {
        AsteroidController instance = null;

        if (_available.Count > 0)
        {
            instance = _available[0];
            _available.Remove(instance);
            _inUse.Add(instance);
        }

        return instance;
    }

    public void ReturnToPool(AsteroidController instance)
    {
        if (!_inUse.Remove(instance))
            return;

        instance.SetActive(false);
        _available.Add(instance);
    }

    private AsteroidController Create()
    {
        GameObject asteroid = Object.Instantiate(_poolObject, _poolContainer);
        AsteroidController instance = new AsteroidController(asteroid);
        instance.SetActive(false);

        return instance;
    }
}
