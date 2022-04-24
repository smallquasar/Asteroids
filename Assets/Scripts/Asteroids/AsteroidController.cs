using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController : ICanSetActive, ICanSetGameObject
    {
        public Action<AsteroidController, AsteroidDisappearingType> OnDestroy;

        public Vector3 CurrentPosition => _asteroid.transform.position;
        public AsteroidType AsteroidType => _asteroidType;

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
            Sprite asteroidSprite = GameData.GetAsteroidSpriteForType(_asteroidType);
            _asteroidView.SetAsteroidImage(asteroidSprite);
            _speed = _asteroidType == AsteroidType.Asteroid ? _asteroidView.Speed : _asteroidView.Speed * 2;
            _maxLifeTime = _asteroidView.MaxLifeTime;
            _asteroidView.AsteroidType = _asteroidType;

            _asteroidView.OnAsteroidUpdate += Update;
            _asteroidView.OnAsteroidDestroy += OnAsteroidDestroy;

        }

        public void Init(Vector3 initPosition)
        {
            _asteroid.transform.position = initPosition;
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
                OnDestroy?.Invoke(this, AsteroidDisappearingType.GotAway);
                return;
            }

            _asteroid.transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnAsteroidDestroy(AsteroidDisappearingType asteroidDisappearingType)
        {
            OnDestroy?.Invoke(this, asteroidDisappearingType);
        }
    }
}
