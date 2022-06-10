using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class ProjectileCreator : IPoolObjectCreator<ProjectileController>
    {
        public WeaponType _weaponType;
        private Transform _parentContainer;
        private WeaponTypeInfo _weaponInfo;
        private EventNotifier _eventNotifier;

        public ProjectileCreator(WeaponType weaponType, Transform parentContainer, List<WeaponTypeInfo> weaponTypes, EventNotifier eventNotifier)
        {
            _weaponType = weaponType;
            _parentContainer = parentContainer;
            _weaponInfo = weaponTypes.FirstOrDefault(x => x.WeaponType == _weaponType);
            _eventNotifier = eventNotifier;
        }

        public ProjectileController Create()
        {
            ProjectileController newProjectile = new ProjectileController(_weaponType, GetPrefab(), _parentContainer);
            _eventNotifier.Attach(newProjectile, Events.EventType.Update);

            return newProjectile;
        }

        private GameObject GetPrefab()
        {
            return _weaponInfo?.ProjectilePrefab ?? null;
        }
    }
}
