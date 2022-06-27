using Assets.Scripts.Events;
using Assets.Scripts.Events.SpaceEventArgs;
using Assets.Scripts.Generation;
using System;
using System.Collections.Generic;
using UnityEngine;
using EventType = Assets.Scripts.Events.EventType;

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

        public override void Update(EventType eventType, EventArgs param)
        {
            if (eventType != EventType.WeaponShot)
            {
                return;
            }

            WeaponShotEventArgs args = param as WeaponShotEventArgs;

            if (args != null && args.WeaponType == WeaponType.MachineGun)
            {
                OnWeaponShot(args.Direction);
            }
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
