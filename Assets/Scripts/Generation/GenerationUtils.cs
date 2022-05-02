using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public static class GenerationUtils
    {
        private static List<SpawnZones> _initSpawnZones = new List<SpawnZones>();
        private static List<SpawnZones> _spawnZones = new List<SpawnZones>();

        public static void SetSpawnZones(List<SpawnZones> initSpawnZones, List<SpawnZones> spawnZones)
        {
            _initSpawnZones = initSpawnZones;
            _spawnZones = spawnZones;
        }

        public static Vector2 GenerateLocation(bool isInitSpawn)
        {
            return GenerateLocationInSpawnZone(isInitSpawn ? _initSpawnZones : _spawnZones);
        }

        public static Vector2 GetRandomDirection()
        {
            return Random.insideUnitCircle.normalized;
        }

        private static Vector2 GenerateLocationInSpawnZone(List<SpawnZones> spawnZones)
        {
            if (spawnZones.Count == 0)
            {
                Debug.Log("List of spawn zones is empty!");
                return Vector2.zero;
            }

            int randomZoneIndex = Random.Range(0, spawnZones.Count);
            SpawnZones currentZone = spawnZones[randomZoneIndex];

            return GetRandomPosition(currentZone.MinX, currentZone.MaxX, currentZone.MinY, currentZone.MaxY);
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
