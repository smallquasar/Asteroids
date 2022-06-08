using Assets.Scripts.Events;
using Assets.Scripts.Events.DestroyEventArgs;
using Assets.Scripts.Generation;
using System;
using UnityEngine;
using EventType = Assets.Scripts.Events.EventType;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipController : ICanSetActive, IObserver
    {
        public Action<SpaceshipController, bool> OnDestroy; //true - уничтожен игроком

        private GameObject _spaceshipObject;
        private Spaceship _spaceship;
        private Transform _spaceshipTransform;

        private float _speed;
        private float _maxLifeTime;
        private Transform _playerTransform;

        private float _timeLeft = 0;

        private int _id;

        public SpaceshipController(Transform playerTransform, GameObject prefab, Transform parentContainer)
        {
            _spaceshipObject = UnityEngine.Object.Instantiate(prefab, parentContainer);
            _playerTransform = playerTransform;
            _spaceshipTransform = _spaceshipObject.transform;
            _spaceship = _spaceshipObject.GetComponent<Spaceship>();

            _speed = _spaceship.Speed;
            _maxLifeTime = _spaceship.MaxLifeTime;

            _id = _spaceshipObject.GetInstanceID();
        }

        public int GetId()
        {
            return _id;
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

        public void Update(EventType eventType, EventArgs param)
        {
            if (eventType == EventType.Update)
            {
                OnUpdateEvent();
            }

            if (eventType == EventType.Destroy)
            {
                DestroyEventArgs args = param as DestroyEventArgs;

                if (args != null && args.ObjectToDestroyId == _id)
                {
                    OnSpaceshipDestroy(isDestroyByPlayer: true);
                }
            }
        }

        private void OnUpdateEvent()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft < 0)
            {
                OnSpaceshipDestroy(isDestroyByPlayer: false);
                return;
            }

            _spaceshipTransform.position = Vector3.MoveTowards(_spaceshipTransform.position, _playerTransform.position, _speed * Time.deltaTime);
        }

        private void OnSpaceshipDestroy(bool isDestroyByPlayer)
        {
            OnDestroy?.Invoke(this, isDestroyByPlayer);
        }
    }
}
