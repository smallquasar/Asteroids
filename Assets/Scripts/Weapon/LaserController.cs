using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class LaserController : WeaponController
    {
        public int AmmunitionCurrentCount => _ammunitionCurrentCount;

        private int _ammunitionMaxCount;
        private int _ammunitionCurrentCount;
        private int _laserOneShotRefillTime;
        private float _laserOneShotRefillTimeCounter;

        private Transform _playerPosition;

        public LaserController(Transform prefabContainer, Transform weaponPosition, Transform playerPosition, int initialCount,
            int laserOneShotRefillTime, List<WeaponTypeInfo> weaponTypes, EventNotifier eventNotifier)
            :base(prefabContainer, weaponPosition, eventNotifier)
        {
            _ammunitionMaxCount = initialCount;
            _ammunitionCurrentCount = initialCount;
            _playerPosition = playerPosition;
            _laserOneShotRefillTime = laserOneShotRefillTime;

            ProjectileCreator creator = new ProjectileCreator(WeaponType.Laser, _prefabContainer, weaponTypes, eventNotifier);
            _projectilesPool = new Pool<ProjectileController>(creator, _ammunitionMaxCount, canExpandPool: false);
        }

        public void LaserAmmunitionCounterUpdate()
        {
            if (_laserOneShotRefillTimeCounter < float.Epsilon)
            {
                return;
            }

            _laserOneShotRefillTimeCounter -= Time.deltaTime;
            if (_laserOneShotRefillTimeCounter < 0)
            {
                SetAmmunitionCurrentCount(_ammunitionCurrentCount + 1);
                if (_ammunitionCurrentCount < _ammunitionMaxCount)
                {
                    _laserOneShotRefillTimeCounter = _laserOneShotRefillTime;
                }
            }
        }

        public override void OnWeaponShot(Vector3 direction)
        {
            if (_ammunitionCurrentCount > 0)
            {
                ProjectileController projectile = _projectilesPool.Get();
                if (projectile == null)
                {
                    return;
                }

                InitProjectile(projectile, direction);

                projectile.SetActive(true);
                projectile.OnDestroy += DestroyProjectile;
                projectile.OnDestroySpaceship += DestroySpaceship;
                projectile.OnDestroyAsteroid += DestroyAsteroid;

                SetAmmunitionCurrentCount(_ammunitionCurrentCount - 1);
            }
        }

        public float LaserAmmunitionRefillTimeCounterCalculate()
        {
            return Mathf.Max(0, _ammunitionMaxCount - _ammunitionCurrentCount - 1) * 3 + _laserOneShotRefillTimeCounter;
        }

        private void SetAmmunitionCurrentCount(int newValue)
        {
            //только начали стрелять из лазера, максимально заполненного снарядами
            //поэтому инициализируем счётчик восстановления одного заряда лазера
            if (_ammunitionCurrentCount == _ammunitionMaxCount && newValue < _ammunitionCurrentCount)
            {
                _laserOneShotRefillTimeCounter = _laserOneShotRefillTime;
            }

            _ammunitionCurrentCount = Mathf.Min(newValue, _ammunitionMaxCount);
            if (_ammunitionCurrentCount == _ammunitionMaxCount)
            {
                _laserOneShotRefillTimeCounter = 0;
            }
        }        

        private void InitProjectile(ProjectileController projectile, Vector3 direction)
        {
            projectile.SetPosition(_weaponPosition.position);
            projectile.Direction = direction;

            Vector3 currentEulerAngles = _playerPosition.transform.eulerAngles;
            Vector3 newRotation = new Vector3(currentEulerAngles.x, currentEulerAngles.y, currentEulerAngles.z);
            projectile.SetRotation(newRotation);
        }

        private void DestroyProjectile(ProjectileController projectile)
        {
            _projectilesPool.ReturnToPool(projectile);
            projectile.OnDestroySpaceship -= DestroySpaceship;
            projectile.OnDestroyAsteroid -= DestroyAsteroid;
            projectile.OnDestroy -= DestroyProjectile;
        }
    }
}
