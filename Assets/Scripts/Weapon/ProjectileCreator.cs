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

        public ProjectileCreator(WeaponType weaponType, Transform parentContainer, List<WeaponTypeInfo> weaponTypes)
        {
            _weaponType = weaponType;
            _parentContainer = parentContainer;
            _weaponInfo = weaponTypes.FirstOrDefault(x => x.WeaponType == _weaponType);
        }

        public ProjectileController Create()
        {
            return new ProjectileController(_weaponType, GetPrefab(), _parentContainer);
        }

        private GameObject GetPrefab()
        {
            return _weaponInfo?.ProjectilePrefab ?? null;
        }
    }
}
