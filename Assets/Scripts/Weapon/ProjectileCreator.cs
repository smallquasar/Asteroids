using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class ProjectileCreator : IPoolObjectCreator<ProjectileController>
    {
        public WeaponType _weaponType;
        private GameObject _prefab;
        private Transform _parentContainer;

        public ProjectileCreator(WeaponType weaponType, GameObject prefab, Transform parentContainer)
        {
            _weaponType = weaponType;
            _prefab = prefab;
            _parentContainer = parentContainer;
        }

        public ProjectileController Create()
        {
            return new ProjectileController(_weaponType, _prefab, _parentContainer);
        }
    }
}
