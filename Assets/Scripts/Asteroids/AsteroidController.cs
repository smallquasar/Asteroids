using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using System;
using UnityEngine;
using EventType = Assets.Scripts.Events.EventType;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController : ICanSetActive, IObserver
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

        private int _id;

        public AsteroidController(AsteroidType asteroidType, GameObject prefab, Transform parentContainer)
        {
            _asteroidObject = UnityEngine.Object.Instantiate(prefab, parentContainer);
            _asteroidType = asteroidType;

            _asteroid = _asteroidObject.GetComponent<Asteroid>();
            _speed = _asteroid.Speed;
            _maxLifeTime = _asteroid.MaxLifeTime;
            _asteroid.AsteroidType = _asteroidType;

            _id = _asteroidObject.GetInstanceID();
        }

        public int GetId()
        {
            return _id;
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

        public void Update(INotifier notifier, EventType eventType)
        {
            if (eventType == EventType.Update)
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft < 0)
                {
                    OnAsteroidDestroy(AsteroidDisappearingType.GotAway);
                    return;
                }

                _asteroidObject.transform.position += _direction * _speed * Time.deltaTime;
            }

            if (eventType == EventType.Destroy)
            {
                IHaveParameter<AsteroidDisappearingType> destroyEvent = notifier as IHaveParameter<AsteroidDisappearingType>;

                if (destroyEvent != null)
                {
                    OnAsteroidDestroy(destroyEvent.GetParameter());
                }
            }
        }

        private void OnAsteroidDestroy(AsteroidDisappearingType asteroidDisappearingType)
        {
            OnDestroy?.Invoke(this, asteroidDisappearingType);
        }
    }
}
