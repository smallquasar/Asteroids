using Assets.Scripts;
using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using Assets.Scripts.Player;
using Assets.Scripts.PlayerInfo;
using Assets.Scripts.Spaceships;
using Assets.Scripts.UI;
using Assets.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 playerStartPosition;
    [SerializeField] private Vector3 playerStartRotation;

    [Header("Asteroids")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int asteroidsInitialCount = 5;
    [SerializeField] private Transform wholeAsteroidsContainer;
    [SerializeField] private Transform asteroidFragmentsContainer;

    [Header("Spaceships")]
    [SerializeField] private GameObject spaceshipPrefab;
    [SerializeField] private int spaceshipsInitialCount = 2;    
    [SerializeField] private Transform spaceshipsContainer;

    [Header("Weapon")]
    [SerializeField] private Transform machineGunContainer;
    [SerializeField] private int machineGunAmmunitionCount = 30;
    [Space(10)]
    [SerializeField] private Transform laserContainer;
    [SerializeField] private int laserAmmunitionInitialCount = 10;    
    [SerializeField] private int laserOneShotRefillTime = 3;
    [Space(10)]
    [SerializeField] private GameObject projectilePrefab;

    [Header("UI")]
    [SerializeField] private GameUIView gameUIView;

    [Header("Game Data")]
    [Space(10)]
    [SerializeField] private List<SpawnZones> spawnZones;
    [SerializeField] private List<AsteroidTypeInfo> asteroidTypes;
    [SerializeField] private List<WeaponTypeInfo> weaponTypes;
    [SerializeField] private List<DestroyPoints> destroyPoints;    

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
        GenerationUtils.SetSpawnZones(spawnZones);
        GameData.SetAsteroidTypes(asteroidTypes);
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
        _laserController = new LaserController(projectilePrefab, laserContainer, weaponTransform, playerTransform, laserAmmunitionInitialCount, laserOneShotRefillTime);

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
        _laserController.LaserAmmunitionCounterUpdate();
        _gameUIController.Update(_statisticsCollector.GetStatistics());
    }

    private void StartGameTimers()
    {
        StartCoroutine(SpawnAsteroids());
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
