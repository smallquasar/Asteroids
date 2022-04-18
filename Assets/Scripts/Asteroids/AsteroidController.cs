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
        private float _speed = 0.5f;

        public AsteroidController()
        {

        }

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
        }

        private void Update()
        {
            _asteroid.transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnAsteroidDestroy()
        {
            OnDestroy?.Invoke(this);
        }
    }
}
