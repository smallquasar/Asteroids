using Assets.Scripts.Asteroids;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ProjectileController : ICanSetActive, ICanSetGameObject
    {
        public Vector3 Direction { get; set; }
        public Action<ProjectileController> OnDestroy;

        private GameObject _projectileObject;
        private Projectile _projectile;
        private WeaponType _weaponType;

        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        public ProjectileController(WeaponType weaponType)
        {
            _weaponType = weaponType;
        }

        public void SetGameObject(GameObject projectile)
        {
            _projectileObject = projectile;
            _projectile = projectile.GetComponent<Projectile>();
            Sprite projectileSprite = GameData.GetWeaponProjectileSpriteForType(_weaponType);
            _projectile.SetProjectileImage(projectileSprite);
            _speed = _projectile.Speed;
            _maxLifeTime = _projectile.MaxLifeTime;

            _projectile.OnProjectileUpdate += Update;
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

        private void Update()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft < 0)
            {
                OnDestroy?.Invoke(this);
                return;
            }

            _projectileObject.transform.position += Direction * _speed * Time.deltaTime;
        }

        private void OnProjectileCrossObject(Collider2D collisionObject)
        {
            if (collisionObject.CompareTag("Enemy"))
            {
                if (collisionObject.TryGetComponent(out AsteroidView asteroid))
                {
                    bool isTotallyDestroy = !(_weaponType == WeaponType.MachineGun && asteroid.AsteroidType == AsteroidType.Asteroid);
                    asteroid.DestroyAsteroid(isTotallyDestroy ? AsteroidDisappearingType.TotallyDestroyed : AsteroidDisappearingType.Shaterred);
                }

                if (collisionObject.TryGetComponent(out Spaceship spaceship))
                {
                    spaceship.DestroySpaceship();
                }

                OnDestroy?.Invoke(this);
            }
        }
    }
}
