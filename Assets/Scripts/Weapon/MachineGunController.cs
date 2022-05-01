using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class MachineGunController : WeaponController
    {
        public MachineGunController(Transform prefabContainer, Transform weaponPosition, int initialCount)
            :base(prefabContainer, weaponPosition)
        {
            _projectilesPool =
                new Pool<ProjectileController>(new ProjectileCreator(WeaponType.MachineGun, _prefabContainer), initialCount, canExpandPool: true);
        }

        public override void OnWeaponShot(Vector3 direction)
        {
            ProjectileController projectile = _projectilesPool.Get();
            if (projectile == null)
            {
                return;
            }

            projectile.SetPosition(_weaponPosition.position);
            projectile.Direction = direction;
            projectile.SetActive(true);
            projectile.OnDestroy += DestroyProjectile;
        }

        private void DestroyProjectile(ProjectileController projectile)
        {
            _projectilesPool.ReturnToPool(projectile);
            projectile.OnDestroy -= DestroyProjectile;
        }
    }
}
