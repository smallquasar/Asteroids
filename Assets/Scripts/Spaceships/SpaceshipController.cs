using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipController : ICanSetActive
    {
        public Action<SpaceshipController, bool> OnDestroy; //true - уничтожен игроком

        private GameObject _spaceshipObject;
        private Spaceship _spaceship;
        private Transform _spaceshipTransform;

        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        private Transform _playerTransform;

        public SpaceshipController(Transform playerTransform, GameObject prefab, Transform parentContainer)
        {
            _spaceshipObject = UnityEngine.Object.Instantiate(prefab, parentContainer);
            _playerTransform = playerTransform;

            _spaceshipTransform = _spaceshipObject.transform;
            _spaceship = _spaceshipObject.GetComponent<Spaceship>();

            _speed = _spaceship.Speed;
            _maxLifeTime = _spaceship.MaxLifeTime;

            _spaceship.OnSpaceshipUpdate += Update;
            _spaceship.OnSpaceshipDestroy += OnSpaceshipDestroy;
        }

        public void Init(Vector3 initPosition)
        {
            _spaceshipTransform.position = initPosition;
        }

        public void SetActive(bool isActive)
        {
            _spaceshipObject.SetActive(isActive);

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
                OnDestroy?.Invoke(this, false);
                return;
            }

            _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, _playerTransform.position, _speed * Time.deltaTime);
        }

        private void OnSpaceshipDestroy()
        {
            OnDestroy?.Invoke(this, true);
        }
    }
}
