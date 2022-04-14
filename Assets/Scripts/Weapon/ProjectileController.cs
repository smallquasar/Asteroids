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
        private float _speed;

        private float _maxLifeTime;
        private float _timeLeft = 0;

        public void SetGameObject(GameObject projectile)
        {
            _projectileObject = projectile;
            _projectile = projectile.GetComponent<Projectile>();
            _speed = _projectile.Speed;
            _maxLifeTime = _projectile.MaxLifeTime;

            _projectile.OnProjectileUpdate += Update;
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

        private void Update()
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft < 0)
            {
                Debug.Log("Time is over!");
                OnDestroy?.Invoke(this);
                return;
            }

            _projectileObject.transform.position += Direction * _speed * Time.deltaTime;
        }
    }
}
