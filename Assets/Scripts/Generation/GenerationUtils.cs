using Assets.Scripts.Asteroids;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GenerationUtils
    {
        private static List<SpawnZones> _spawnZones = new List<SpawnZones>();

        public static void SetSpawnZones(List<SpawnZones> spawnZones)
        {
            _spawnZones = spawnZones;
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
