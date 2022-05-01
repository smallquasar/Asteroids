using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController : ICanSetActive
    {
        public Action<AsteroidController, AsteroidDisappearingType> OnDestroy;

        public Vector3 CurrentPosition => _asteroidObject.transform.position;
        public AsteroidType AsteroidType => _asteroidType;

        private GameObject _asteroidObject;
        private Asteroid _asteroid;
        private AsteroidType _asteroidType;

        private Vector3 _direction;
        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        public AsteroidController(AsteroidType asteroidType, GameObject prefab, Transform parentContainer)
        {
            _asteroidObject = UnityEngine.Object.Instantiate(prefab, parentContainer);
            _asteroidType = asteroidType;

            _asteroid = _asteroidObject.GetComponent<Asteroid>();
            _speed = _asteroid.Speed;
            _maxLifeTime = _asteroid.MaxLifeTime;
            _asteroid.AsteroidType = _asteroidType;

            _asteroid.OnAsteroidUpdate += Update;
            _asteroid.OnAsteroidDestroy += OnAsteroidDestroy;
        }

        public void Init(Vector3 initPosition)
        {
            _asteroidObject.transform.position = initPosition;
            _direction = GenerationUtils.GetRandomDirection();
        }

        public void SetActive(bool isActive)
        {
            _asteroidObject.SetActive(isActive);

            if (isActive)
            {
                _timeLeft = _maxLifeTime;
            }
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft < 0)
            {
                OnDestroy?.Invoke(this, AsteroidDisappearingType.GotAway);
                return;
            }

            _asteroidObject.transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnAsteroidDestroy(AsteroidDisappearingType asteroidDisappearingType)
        {
            OnDestroy?.Invoke(this, asteroidDisappearingType);
        }
    }
}
