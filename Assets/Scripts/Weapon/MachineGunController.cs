using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class MachineGunController : WeaponController
    {
        public MachineGunController(Transform prefabContainer, Transform weaponPosition, int initialCount, List<WeaponTypeInfo> weaponTypes,
            EventNotifier eventNotifier)
            :base(prefabContainer, weaponPosition, eventNotifier)
        {
            ProjectileCreator creator = new ProjectileCreator(WeaponType.MachineGun, _prefabContainer, weaponTypes, eventNotifier);
            _projectilesPool = new Pool<ProjectileController>(creator, initialCount, canExpandPool: true);
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
            projectile.OnDestroySpaceship += DestroySpaceship;
            projectile.OnDestroyAsteroid += DestroyAsteroid;
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
