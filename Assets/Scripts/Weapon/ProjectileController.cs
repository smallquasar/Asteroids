using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using Assets.Scripts.Spaceships;
using System;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class ProjectileController : ICanSetActive
    {
        public Vector3 Direction { get; set; }
        public Action<ProjectileController> OnDestroy;

        private GameObject _projectile;
        private ProjectileView _projectileView;
        private WeaponType _weaponType;

        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        public ProjectileController(WeaponType weaponType, GameObject prefab, Transform poolContainer)
        {
            _projectile = UnityEngine.Object.Instantiate(prefab, poolContainer);
            _weaponType = weaponType;

            _projectileView = _projectile.GetComponent<ProjectileView>();
            //Sprite projectileSprite = GameData.GetWeaponProjectileSpriteForType(_weaponType);
            //_projectileView.SetProjectileImage(projectileSprite);
            _speed = _projectileView.Speed;
            _maxLifeTime = _projectileView.MaxLifeTime;

            _projectileView.OnProjectileUpdate += Update;
            _projectileView.OnProjectileCrossObject += OnProjectileCrossObject;
        }

        public void SetActive(bool isActive)
        {
            _projectile.SetActive(isActive);

            if (isActive)
            {
                _timeLeft = _maxLifeTime;
            }
        }

        public void SetPosition(Vector3 position)
        {
            _projectile.transform.position = position;
        }

        public void SetRotation(Vector3 rotation)
        {
            _projectile.transform.eulerAngles = rotation;
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft < 0)
            {
                OnDestroy?.Invoke(this);
                return;
            }

            _projectile.transform.position += Direction * _speed * Time.deltaTime;
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
