using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class WeaponController
    {
        protected Transform _prefabContainer;
        protected Transform _weaponPosition;

        protected Pool<ProjectileController> _projectilesPool;

        public WeaponController(Transform prefabContainer, Transform weaponPosition)
        {
            _prefabContainer = prefabContainer;
            _weaponPosition = weaponPosition;
        }

        public abstract void OnWeaponShot(Vector3 direction);
    }
}
