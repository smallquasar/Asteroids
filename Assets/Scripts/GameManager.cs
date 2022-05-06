using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using Assets.Scripts.LevelInfo;
using Assets.Scripts.Player;
using Assets.Scripts.PlayerInfo;
using Assets.Scripts.SpaceObjectsInfo;
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
    [SerializeField] private int asteroidsInitialCount = 10;
    [SerializeField] private float asteroidAppearanceTime = 0.2f;
    [SerializeField] private Transform wholeAsteroidsContainer;
    [SerializeField] private Transform asteroidFragmentsContainer;

    [Header("Spaceships")]
    [SerializeField] private int spaceshipsInitialCount = 2;    
    [SerializeField] private Transform spaceshipsContainer;
    [Space(10)]
    [SerializeField] private int spaceshipAppearanceTimeFrom = 7;
    [SerializeField] private int spaceshipAppearanceTimeTo = 25;

    [Header("Weapon")]
    [SerializeField] private Transform machineGunContainer;
    [SerializeField] private int machineGunAmmunitionInitialCount = 30;
    [Space(10)]
    [SerializeField] private Transform laserContainer;
    [SerializeField] private int laserAmmunitionInitialCount = 5;    
    [SerializeField] private int laserOneShotRefillTime = 3;

    [Header("UI")]
    [SerializeField] private GameUIView gameUIView;

    [Header("Game Data")]
    [Space(10)]
    [SerializeField] private SpaceObjectVariants spaceshipVariants;    
    [SerializeField] private List<AsteroidVariants> asteroidVariants;
    [SerializeField] private List<WeaponTypeInfo> weaponTypes;
    [SerializeField] private List<DestroyPoints> destroyPoints;
    [Space(10)]
    [SerializeField] private List<SpawnZones> spawnZones;
    [SerializeField] private List<SpawnZones> initSpawnZones;

    private AsteroidsGenerator _asteroidsGenerator;
    private SpaceshipsGenerator _spaceshipsGenerator;
    private PlayerController _playerController;

    private MachineGunController _machineGunController;
    private LaserController _laserController;

    private PointsController _pointsController;
    private StatisticsCollector _statisticsCollector;

    private GameUIController _gameUIController;

    private PlayerInput _playerInput;

    private EventManager _eventManager;
    private DestroyEventManager _destroyEventManager;
    private DestroyEventManagerWithParameters<AsteroidDisappearingType> _destroyEventManagerWithParameters;

    public void Awake()
    {
        _playerInput = new PlayerInput();
    }

    public void Start()
    {
        _eventManager = new EventManager();
        _destroyEventManager = new DestroyEventManager();
        _destroyEventManagerWithParameters = new DestroyEventManagerWithParameters<AsteroidDisappearingType>();

        GenerationUtils.SetSpawnZones(initSpawnZones, spawnZones);

        CreatePlayer();

        Transform weaponTransform = _playerController.WeaponTransform;
        Transform playerTransform = _playerController.PlayerTransform;

        _pointsController = new PointsController(destroyPoints);

        CreateSpaceObjectGenerators(playerTransform);
        CreateWeapon(playerTransform, weaponTransform);  

        _statisticsCollector = new StatisticsCollector(_playerController, _laserController);

        CreateGameUI();

        _playerController.OnWeaponShot += OnWeaponShot;
        _playerController.OnDie += GameOver;        

        StartGameTimers();
    }

    public void Update()
    {
        _eventManager.Notify();

        
        _laserController.LaserAmmunitionCounterUpdate();

        _gameUIController.Update(_statisticsCollector.GetStatistics());
    }

    private void CreatePlayer()
    {
        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, _playerInput, worldHeight, worldWidth);
        _eventManager.Attach(_playerController);
        _playerController.SetPlayerPosition(playerStartPosition, playerStartRotation);        
    }

    private void CreateSpaceObjectGenerators(Transform playerTransform)
    {
        _asteroidsGenerator = new AsteroidsGenerator(wholeAsteroidsContainer, asteroidFragmentsContainer, asteroidsInitialCount, asteroidVariants, _eventManager,
            _destroyEventManagerWithParameters);
        _asteroidsGenerator.OnGotAchievement += OnGotAchievement;
        _asteroidsGenerator.Start();

        _spaceshipsGenerator = new SpaceshipsGenerator(spaceshipsContainer, playerTransform, spaceshipsInitialCount, spaceshipVariants, _eventManager, _destroyEventManager);
        _spaceshipsGenerator.OnGotAchievement += OnGotAchievement;
    }

    private void CreateWeapon(Transform playerTransform, Transform weaponTransform)
    {
        _machineGunController = new MachineGunController(machineGunContainer, weaponTransform, machineGunAmmunitionInitialCount, weaponTypes, _eventManager,
            _destroyEventManager, _destroyEventManagerWithParameters);
        _laserController = new LaserController(laserContainer, weaponTransform, playerTransform, laserAmmunitionInitialCount, laserOneShotRefillTime,
            weaponTypes, _eventManager, _destroyEventManager, _destroyEventManagerWithParameters);
    }

    private void CreateGameUI()
    {
        _gameUIController = new GameUIController(gameUIView);
        _gameUIController.OnContinueGame += ContinueGame;
        _gameUIController.OnExitGame += ExitGame;
    }

    private void StartGameTimers()
    {
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(SpawnSpaceships());
    }    

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            _asteroidsGenerator.SpawnNewAsteroid(isInitSpawn: false);
            yield return new WaitForSeconds(asteroidAppearanceTime);
        }
    }

    private IEnumerator SpawnSpaceships()
    {
        while (true)
        {
            int timeForWait = Random.Range(spaceshipAppearanceTimeFrom, spaceshipAppearanceTimeTo + 1);            
            yield return new WaitForSeconds(timeForWait);

            _spaceshipsGenerator.SpawnNewShip();
        }
    }

    private void OnGotAchievement(Achievement achievement)
    {
        _pointsController.CalculatePoints(achievement);
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

    private void GameOver()
    {
        Time.timeScale = 0;
        _gameUIController.PlayerDie(_pointsController.Points);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
