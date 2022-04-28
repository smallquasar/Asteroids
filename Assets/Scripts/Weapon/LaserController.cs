using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class LaserController : WeaponController
    {
        public int AmmunitionCurrentCount => _ammunitionCurrentCount;

        private int _ammunitionMaxCount;
        private int _ammunitionCurrentCount;

        private Transform _playerPosition;

        public LaserController(GameObject projectilePrefab, Transform prefabContainer, Transform weaponPosition, Transform playerPosition, int initialCount)
            :base(projectilePrefab, prefabContainer, weaponPosition)
        {
            _ammunitionMaxCount = initialCount;
            _ammunitionCurrentCount = initialCount;
            _playerPosition = playerPosition;

            _projectilesPool =
                new Pool<ProjectileController>(new ProjectileCreator(WeaponType.Laser, _projectilePrefab, _prefabContainer), _ammunitionMaxCount, canExpandPool: false);
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
                _ammunitionCurrentCount--;
            }
        }

        public void CheckLaserAmmunition()
        {
            if (_ammunitionCurrentCount < _ammunitionMaxCount)
            {
                _ammunitionCurrentCount++;
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
            projectile.OnDestroy -= DestroyProjectile;
        }
    }
}
