using Assets.Scripts;
using Assets.Scripts.Asteroids;
using Assets.Scripts.Player;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector3 playerStartPosition;
    [SerializeField] Vector3 playerStartRotation;

    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] int asteroidsInitialCount = 5;
    [SerializeField] GameObject spaceshipPrefab;
    [SerializeField] int spaceshipsInitialCount = 2;

    [SerializeField] Transform wholeAsteroidsContainer;
    [SerializeField] Transform asteroidFragmentsContainer;
    [SerializeField] Transform spaceshipsContainer;
    [SerializeField] Transform machineGunContainer;
    [SerializeField] Transform laserContainer;

    [SerializeField] GameObject projectilePrefab;    
    [SerializeField] int machineGunAmmunitionCount = 30;
    [SerializeField] int laserAmmunitionInitialCount = 10;
    
    [SerializeField] private List<SpawnZones> _spawnZones;
    [SerializeField] private List<AsteroidTypeInfo> _asteroidTypes;
    [SerializeField] private List<WeaponTypeInfo> weaponTypes;
    [SerializeField] private List<DestroyPoints> destroyPoints;

    [SerializeField] private GameUIView gameUIView;

    private AsteroidsGenerator _asteroidsGenerator;
    private SpaceshipsGenerator _spaceshipsGenerator;
    private PlayerController _playerController;

    private MachineGunController _machineGunController;
    private LaserController _laserController;

    private PointsController _pointsController;
    private StatisticsCollector _statisticsCollector;

    private GameUIController _gameUIController;

    private PlayerInput _playerInput;

    public void Awake()
    {
        _playerInput = new PlayerInput();
    }

    public void Start()
    {
        GenerationUtils.SetSpawnZones(_spawnZones);
        GameData.SetAsteroidTypes(_asteroidTypes);
        GameData.SetWeaponTypes(weaponTypes);
        GameData.SetDestroyPoints(destroyPoints);

        _asteroidsGenerator = new AsteroidsGenerator(asteroidPrefab, wholeAsteroidsContainer, asteroidFragmentsContainer, asteroidsInitialCount);
        _asteroidsGenerator.OnGotAchievement += OnGotAchievement;
        _asteroidsGenerator.Start();        

        _pointsController = new PointsController();

        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, _playerInput, worldHeight, worldWidth);
        _playerController.SetPlayerPosition(playerStartPosition, playerStartRotation);
        Transform weaponTransform = _playerController.WeaponTransform;
        Transform playerTransform = _playerController.PlayerTransform;

        _spaceshipsGenerator = new SpaceshipsGenerator(spaceshipPrefab, spaceshipsContainer, playerTransform, spaceshipsInitialCount);
        _spaceshipsGenerator.OnGotAchievement += OnGotAchievement;

        _machineGunController = new MachineGunController(projectilePrefab, machineGunContainer, weaponTransform, machineGunAmmunitionCount);
        _laserController = new LaserController(projectilePrefab, laserContainer, weaponTransform, playerTransform, laserAmmunitionInitialCount);

        _statisticsCollector = new StatisticsCollector(_playerController, _laserController);

        _gameUIController = new GameUIController(gameUIView);
        _gameUIController.OnContinueGame += ContinueGame;
        _gameUIController.OnExitGame += ExitGame;

        _playerController.OnWeaponShot += OnWeaponShot;
        _playerController.OnDie += GameOver;

        StartGameTimers();
    }

    public void Update()
    {
        _gameUIController.Update(_statisticsCollector.GetStatistics());
    }

    private void StartGameTimers()
    {
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(CheckLaserAmmunition());
        StartCoroutine(SpawnSpaceships());
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

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            _asteroidsGenerator.SpawnNewAsteroid();
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator SpawnSpaceships()
    {
        while (true)
        {
            int timeForWait = Random.Range(7, 25);            
            yield return new WaitForSeconds(timeForWait);

            _spaceshipsGenerator.SpawnNewShip();
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

    private void OnGotAchievement(Achievement achievement)
    {
        _pointsController.CalculatePoints(achievement);
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        _gameUIController.PlayerDie(_pointsController.Points);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
        ReloadGame();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        _playerInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Gameplay.Disable();
    }
}
