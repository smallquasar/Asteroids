using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController
    {
        private GameObject _asteroid;
        private AsteroidView _asteroidView;

        private Vector3 _direction;
        private float _speed = 0.5f;

        public AsteroidController(GameObject asteroid)
        {
            _asteroid = asteroid;
            _asteroidView = _asteroid.GetComponent<AsteroidView>();            
        }

        public void Init()
        {
            _asteroid.transform.position = GenerationUtils.GenerateLocation();
            _direction = GenerationUtils.GetRandomDirection();
            _asteroidView.OnAsteroidUpdate += Update;
        }

        public void SetActive(bool isActive)
        {
            _asteroid.SetActive(isActive);
        }

        private void Update()
        {
            _asteroid.transform.position += _direction * _speed * Time.deltaTime;
        }
    }
}
