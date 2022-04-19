using Assets.Scripts.Generation;
using Assets.Scripts.Player;

namespace Assets.Scripts.Weapon
{
    public class ProjectileCreator : IPoolObjectCreator<ProjectileController>
    {
        public WeaponType _weaponType;

        public ProjectileCreator(WeaponType weaponType)
        {
            _weaponType = weaponType;
        }

        public ProjectileController Create()
        {
            return new ProjectileController(_weaponType);
        }
    }
}
