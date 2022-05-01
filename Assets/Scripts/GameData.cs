using Assets.Scripts.Asteroids;
using Assets.Scripts.PlayerInfo;
using Assets.Scripts.SpaceObjectsInfo;
using Assets.Scripts.Weapon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GameData
    {
        private static SpaceObjectVariants _spaceshipVariants;
        private static List<AsteroidVariants> _asteroidVariants = new List<AsteroidVariants>();
        private static List<WeaponTypeInfo> _weaponTypes = new List<WeaponTypeInfo>();
        private static List<DestroyPoints> _destroyPoints = new List<DestroyPoints>();

        public static void SetSpaceshipVariants(SpaceObjectVariants spaceshipVariants)
        {
            _spaceshipVariants = spaceshipVariants;
        }

        public static GameObject GetSpaceshipPrefab()
        {
            return _spaceshipVariants?.GetRandomVariant() ?? null;
        }

        public static void SetAsteroidVariants(List<AsteroidVariants> asteroidVariants)
        {
            _asteroidVariants = asteroidVariants;
        }

        public static GameObject GetAsteroidVariantForType(AsteroidType asteroidType)
        {
            AsteroidVariants asteroidInfo = _asteroidVariants.FirstOrDefault(x => x.AsteroidType == asteroidType);
            return asteroidInfo?.GetRandomVariant() ?? null;
        }

        public static void SetWeaponTypes(List<WeaponTypeInfo> weaponTypes)
        {
            _weaponTypes = weaponTypes;
        }

        public static GameObject GetWeaponProjectilePrefabForType(WeaponType weaponType)
        {
            WeaponTypeInfo weaponInfo = _weaponTypes.FirstOrDefault(x => x.WeaponType == weaponType);
            return weaponInfo?.ProjectilePrefab ?? null;
        }

        public static void SetDestroyPoints(List<DestroyPoints> destroyPoints)
        {
            _destroyPoints = destroyPoints;
        }

        public static int GetDestroyPointsForAchievement(Achievement achievement)
        {
            DestroyPoints destroyPoints = _destroyPoints.FirstOrDefault(x => x.Achievement == achievement);
            return destroyPoints?.Points ?? 0;
        }
    }
}
