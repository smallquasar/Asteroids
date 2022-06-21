using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using Assets.Scripts.PlayerInfo;
using Assets.Scripts.SpaceObjectsInfo;
using Assets.Scripts.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelInfo
{
    [CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObjects/LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private Vector3 playerStartRotation;

        [Header("Asteroids")]
        [SerializeField] private int asteroidsInitialCount = 10;
        [SerializeField] private float asteroidAppearanceTime = 0.2f;

        [Header("Spaceships")]
        [SerializeField] private int spaceshipsInitialCount = 2;
        [SerializeField] private int spaceshipAppearanceTimeFrom = 7;
        [SerializeField] private int spaceshipAppearanceTimeTo = 25;

        [Header("Weapon")]
        [SerializeField] private int machineGunAmmunitionInitialCount = 30;
        [SerializeField] private int laserAmmunitionInitialCount = 5;
        [SerializeField] private int laserOneShotRefillTime = 3;

        [Header("Game Data")]
        [Space(10)]
        [SerializeField] private SpaceObjectVariants spaceshipVariants;
        [SerializeField] private List<AsteroidVariants> asteroidVariants;
        [SerializeField] private List<WeaponTypeInfo> weaponTypes;
        [SerializeField] private List<DestroyPoints> destroyPoints;
        [Space(10)]
        [SerializeField] private List<SpawnZones> spawnZones;
        [SerializeField] private List<SpawnZones> initSpawnZones;

        public GameObject PlayerPrefab => playerPrefab;
        public Vector3 PlayerStartPosition => playerStartPosition;
        public Vector3 PlayerStartRotation => playerStartRotation;
        public int AsteroidsInitialCount => asteroidsInitialCount;
        public float AsteroidAppearanceTime => asteroidAppearanceTime;
        public int SpaceshipsInitialCount => spaceshipsInitialCount;
        public int SpaceshipAppearanceTimeFrom => spaceshipAppearanceTimeFrom;
        public int SpaceshipAppearanceTimeTo => spaceshipAppearanceTimeTo;
        public int MachineGunAmmunitionInitialCount => machineGunAmmunitionInitialCount;
        public int LaserAmmunitionInitialCount => laserAmmunitionInitialCount;
        public int LaserOneShotRefillTime => laserOneShotRefillTime;
        public SpaceObjectVariants SpaceshipVariants => spaceshipVariants;
        public List<AsteroidVariants> AsteroidVariants => asteroidVariants;
        public List<WeaponTypeInfo> WeaponTypes => weaponTypes;
        public List<DestroyPoints> DestroyPoints => destroyPoints;
        public List<SpawnZones> SpawnZones => spawnZones;
        public List<SpawnZones> InitSpawnZones => initSpawnZones;        
    }
}
