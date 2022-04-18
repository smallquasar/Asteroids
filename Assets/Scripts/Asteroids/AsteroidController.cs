using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController : ICanSetActive, ICanSetGameObject
    {
        public Action<AsteroidController> OnDestroy;

        private GameObject _asteroid;
        private AsteroidView _asteroidView;
        private AsteroidType _asteroidType;

        private Vector3 _direction;
        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        public AsteroidController(AsteroidType asteroidType)
        {
            _asteroidType = asteroidType;
        }

        public void SetGameObject(GameObject asteroid)
        {
            _asteroid = asteroid;
            _asteroidView = _asteroid.GetComponent<AsteroidView>();
            Sprite asteroidSprite = GenerationUtils.GetAsteroidSpriteForType(_asteroidType);
            _asteroidView.SetAsteroidImage(asteroidSprite);
            _speed = _asteroidView.Speed;
            _maxLifeTime = _asteroidView.MaxLifeTime;

            _asteroidView.OnAsteroidUpdate += Update;
            _asteroidView.OnAsteroidDestroy += OnAsteroidDestroy;

        }

        public void Start()
        {
            _asteroid.transform.position = GenerationUtils.GenerateLocation();
            _direction = GenerationUtils.GetRandomDirection();
        }

        public void SetActive(bool isActive)
        {
            _asteroid.SetActive(isActive);

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
                OnDestroy?.Invoke(this);
                return;
            }

            _asteroid.transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnAsteroidDestroy()
        {
            OnDestroy?.Invoke(this);
        }
    }
}
