using Assets.Scripts;
using Assets.Scripts.Asteroids;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidsContainer;

    [SerializeField] GameObject laserProjectilePrefab;
    [SerializeField] int asteroidsInitialCount = 5;
    [SerializeField] int laserAmmunitionInitialCount = 10;
    
    [SerializeField] private List<SpawnZones> _spawnZones;
    [SerializeField] private List<AsteroidTypeInfo> _asteroidTypes;

    private AsteroidsGenerator _asteroidsGenerator;
    private PlayerController _playerController;
    private LaserController _laserController;

    public void Start()
    {
        GenerationUtils.SetSpawnZones(_spawnZones);
        GenerationUtils.SetAsteroidTypes(_asteroidTypes);
        _asteroidsGenerator = new AsteroidsGenerator(asteroidPrefab, asteroidsContainer, asteroidsInitialCount);
        _asteroidsGenerator.Start();

        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, worldHeight, worldWidth);
        Transform weaponTransform = _playerController.WeaponTransform;

        _laserController = new LaserController(laserProjectilePrefab, weaponTransform, laserAmmunitionInitialCount);

        _playerController.OnLaserShot += _laserController.OnLaserShot;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(CheckLaserAmmunition());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            _asteroidsGenerator.SpawnNew();
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator CheckLaserAmmunition()
    {
        while (true)
        {
            _laserController.CheckLaserAmmunition();

            yield return new WaitForSeconds(3);
        }
    }
}
