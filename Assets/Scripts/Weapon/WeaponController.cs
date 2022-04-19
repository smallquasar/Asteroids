using Assets.Scripts.Generation;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class WeaponController
    {
        protected GameObject _projectilePrefab;
        protected Transform _prefabContainer;
        protected Transform _weaponPosition;

        protected Pool<ProjectileController> _projectilesPool;

        public WeaponController(GameObject projectilePrefab, Transform prefabContainer, Transform weaponPosition)
        {
            _projectilePrefab = projectilePrefab;
            _prefabContainer = prefabContainer;
            _weaponPosition = weaponPosition;
        }

        public abstract void OnWeaponShot(Vector3 direction);
    }
}
