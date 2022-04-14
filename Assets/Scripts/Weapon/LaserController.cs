using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class LaserController
    {
        public int AmmunitionCurrentCount => _ammunitionCurrentCount;

        private GameObject _projectilePrefab;
        private Transform _prefabContainer;

        private int _ammunitionMaxCount;
        private int _ammunitionCurrentCount;

        private Pool<ProjectileController> _laserPool;

        public LaserController(GameObject projectilePrefab, Transform prefabContainer, int initialCount)
        {
            _projectilePrefab = projectilePrefab;
            _prefabContainer = prefabContainer;
            _ammunitionMaxCount = initialCount;
            _ammunitionCurrentCount = initialCount;

            _laserPool = new Pool<ProjectileController>(_projectilePrefab, _prefabContainer, _ammunitionMaxCount, canExpandPool: false);
        }

        public void OnLaserShot(Vector3 direction)
        {
            if (_ammunitionCurrentCount > 0)
            {
                ProjectileController projectile = _laserPool.Get();
                if (projectile == null)
                {
                    return;
                }

                projectile.SetPosition(_prefabContainer.position);
                projectile.Direction = direction;
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

        private void DestroyProjectile(ProjectileController projectile)
        {
            _laserPool.ReturnToPool(projectile);
            projectile.OnDestroy -= DestroyProjectile;
        }
    }
}
