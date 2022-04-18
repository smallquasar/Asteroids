using Assets.Scripts.Asteroids;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GenerationUtils
    {
        private static List<SpawnZones> _spawnZones = new List<SpawnZones>();
        private static List<AsteroidTypeInfo> _asteroidTypes = new List<AsteroidTypeInfo>();

        public static void SetSpawnZones(List<SpawnZones> spawnZones)
        {
            _spawnZones = spawnZones;
        }

        public static void SetAsteroidTypes(List<AsteroidTypeInfo> asteroidTypes)
        {
            _asteroidTypes = asteroidTypes;
        }

        public static Sprite GetAsteroidSpriteForType(AsteroidType asteroidType)
        {
            AsteroidTypeInfo asteroidInfo = _asteroidTypes.FirstOrDefault(x => x.AsteroidType == asteroidType);
            return asteroidInfo?.AsteroidSprite ?? null;
        }

        public static Vector2 GenerateLocation()
        {
            if (_spawnZones.Count == 0)
            {
                Debug.Log("List of spawn zones is empty!");
                return Vector2.zero;
            }

            int randomZoneIndex = Random.Range(0, _spawnZones.Count);
            SpawnZones currentZone = _spawnZones[randomZoneIndex];

            return GetRandomPosition(currentZone.MinX, currentZone.MaxX, currentZone.MinY, currentZone.MaxY);
        }

        public static Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }

        /// <summary>
        /// Get random position in rectangular area
        /// </summary>
        private static Vector2 GetRandomPosition(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }
    }
}
