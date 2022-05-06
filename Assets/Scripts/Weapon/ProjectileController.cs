using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using Assets.Scripts.Spaceships;
using System;
using UnityEngine;
using EventType = Assets.Scripts.Events.EventType;

namespace Assets.Scripts.Weapon
{
    public class ProjectileController : ICanSetActive, IObserver
    {
        public Vector3 Direction { get; set; }

        public Action<ProjectileController> OnDestroy;
        public Action<int> OnDestroySpaceship;
        public Action<int, AsteroidDisappearingType> OnDestroyAsteroid;

        private GameObject _projectileObject;
        private Projectile _projectile;
        private WeaponType _weaponType;

        private float _speed;
        private float _maxLifeTime;

        private float _timeLeft = 0;

        public ProjectileController(WeaponType weaponType, GameObject prefab, Transform poolContainer)
        {
            _projectileObject = UnityEngine.Object.Instantiate(prefab, poolContainer);
            _projectile = _projectileObject.GetComponent<Projectile>();
            _speed = _projectile.Speed;
            _maxLifeTime = _projectile.MaxLifeTime;

            _weaponType = weaponType;

            _projectile.OnProjectileCrossObject += OnProjectileCrossObject;
        }

        public void SetActive(bool isActive)
        {
            _projectileObject.SetActive(isActive);

            if (isActive)
            {
                _timeLeft = _maxLifeTime;
            }
        }

        public void SetPosition(Vector3 position)
        {
            _projectileObject.transform.position = position;
        }

        public void SetRotation(Vector3 rotation)
        {
            _projectileObject.transform.eulerAngles = rotation;
        }

        public void Update(INotifier notifier, EventType eventType)
        {
            if (eventType == EventType.Update)
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft < 0)
                {
                    OnDestroy?.Invoke(this);
                    return;
                }

                _projectileObject.transform.position += Direction * _speed * Time.deltaTime;
            }
        }

        private void OnProjectileCrossObject(Collider2D collisionObject)
        {
            if (collisionObject.CompareTag("Enemy"))
            {
                if (collisionObject.TryGetComponent(out Asteroid asteroid))
                {
                    bool isTotallyDestroy = !(_weaponType == WeaponType.MachineGun && asteroid.AsteroidType == AsteroidType.Asteroid);
                    AsteroidDisappearingType disappearingType = isTotallyDestroy ? AsteroidDisappearingType.TotallyDestroyed : AsteroidDisappearingType.Shaterred;

                    OnDestroyAsteroid?.Invoke(asteroid.gameObject.GetInstanceID(), disappearingType);
                }

                if (collisionObject.TryGetComponent(out Spaceship spaceship))
                {
                    OnDestroySpaceship?.Invoke(spaceship.gameObject.GetInstanceID());
                }

                OnDestroy?.Invoke(this);
            }
        }
    }
}
