using Assets.Scripts;
using Assets.Scripts.Asteroids;
using Assets.Scripts.Player;
using Assets.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject asteroidPrefab;

    [SerializeField] Transform asteroidsContainer;
    [SerializeField] Transform machineGunContainer;
    [SerializeField] Transform laserContainer;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int asteroidsInitialCount = 5;
    [SerializeField] int machineGunAmmunitionCount = 30;
    [SerializeField] int laserAmmunitionInitialCount = 10;
    
    [SerializeField] private List<SpawnZones> _spawnZones;
    [SerializeField] private List<AsteroidTypeInfo> _asteroidTypes;
    [SerializeField] private List<WeaponTypeInfo> weaponTypes;

    private AsteroidsGenerator _asteroidsGenerator;
    private PlayerController _playerController;

    private MachineGunController _machineGunController;
    private LaserController _laserController;

    public void Start()
    {
        GenerationUtils.SetSpawnZones(_spawnZones);
        GameData.SetAsteroidTypes(_asteroidTypes);
        GameData.SetWeaponTypes(weaponTypes);

        _asteroidsGenerator = new AsteroidsGenerator(asteroidPrefab, asteroidsContainer, asteroidsInitialCount);
        _asteroidsGenerator.Start();

        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, worldHeight, worldWidth);
        Transform weaponTransform = _playerController.WeaponTransform;
        Transform playerTransform = _playerController.PlayerTransform;

        _machineGunController = new MachineGunController(projectilePrefab, machineGunContainer, weaponTransform, machineGunAmmunitionCount);
        _laserController = new LaserController(projectilePrefab, laserContainer, weaponTransform, playerTransform, laserAmmunitionInitialCount);

        _playerController.OnWeaponShot += OnWeaponShot;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(CheckLaserAmmunition());
    }

    private void OnWeaponShot(Vector3 direction, WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.MachineGun:
                _machineGunController.OnWeaponShot(direction);
                break;
            case WeaponType.Laser:
                _laserController.OnWeaponShot(direction);
                break;
        }
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
