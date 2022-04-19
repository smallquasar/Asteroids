using Assets.Scripts.Asteroids;
using Assets.Scripts.Weapon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GameData
    {
        private static List<AsteroidTypeInfo> _asteroidTypes = new List<AsteroidTypeInfo>();
        private static List<WeaponTypeInfo> _weaponTypes = new List<WeaponTypeInfo>();

        public static void SetAsteroidTypes(List<AsteroidTypeInfo> asteroidTypes)
        {
            _asteroidTypes = asteroidTypes;
        }

        public static Sprite GetAsteroidSpriteForType(AsteroidType asteroidType)
        {
            AsteroidTypeInfo asteroidInfo = _asteroidTypes.FirstOrDefault(x => x.AsteroidType == asteroidType);
            return asteroidInfo?.AsteroidSprite ?? null;
        }

        public static void SetWeaponTypes(List<WeaponTypeInfo> weaponTypes)
        {
            _weaponTypes = weaponTypes;
        }

        public static Sprite GetWeaponProjectileSpriteForType(WeaponType weaponType)
        {
            WeaponTypeInfo weaponInfo = _weaponTypes.FirstOrDefault(x => x.WeaponType == weaponType);
            return weaponInfo?.ProjectileSprite ?? null;
        }
    }
}
