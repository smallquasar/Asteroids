using Assets.Scripts;
using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
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
    [SerializeField] private int asteroidsInitialCount = 5;
    [SerializeField] private Transform wholeAsteroidsContainer;
    [SerializeField] private Transform asteroidFragmentsContainer;

    [Header("Spaceships")]
    [SerializeField] private int spaceshipsInitialCount = 2;    
    [SerializeField] private Transform spaceshipsContainer;

    [Header("Weapon")]
    [SerializeField] private Transform machineGunContainer;
    [SerializeField] private int machineGunAmmunitionCount = 30;
    [Space(10)]
    [SerializeField] private Transform laserContainer;
    [SerializeField] private int laserAmmunitionInitialCount = 10;    
    [SerializeField] private int laserOneShotRefillTime = 3;

    [Header("UI")]
    [SerializeField] private GameUIView gameUIView;

    [Header("Game Data")]
    [Space(10)]
    [SerializeField] private SpaceObjectVariants spaceshipVariants;
    [SerializeField] private List<SpawnZones> spawnZones;
    [SerializeField] private List<AsteroidVariants> asteroidVariants;
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
        SetGameData();

        CreatePlayer();

        Transform weaponTransform = _playerController.WeaponTransform;
        Transform playerTransform = _playerController.PlayerTransform;

        _pointsController = new PointsController();

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
        _laserController.LaserAmmunitionCounterUpdate();
        _gameUIController.Update(_statisticsCollector.GetStatistics());
    }

    private void SetGameData()
    {
        GenerationUtils.SetSpawnZones(spawnZones);

        GameData.SetSpaceshipVariants(spaceshipVariants);
        GameData.SetAsteroidVariants(asteroidVariants);
        GameData.SetWeaponTypes(weaponTypes);
        GameData.SetDestroyPoints(destroyPoints);
    }

    private void CreatePlayer()
    {
        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, _playerInput, worldHeight, worldWidth);
        _playerController.SetPlayerPosition(playerStartPosition, playerStartRotation);        
    }

    private void CreateSpaceObjectGenerators(Transform playerTransform)
    {
        _asteroidsGenerator = new AsteroidsGenerator(wholeAsteroidsContainer, asteroidFragmentsContainer, asteroidsInitialCount);
        _asteroidsGenerator.OnGotAchievement += OnGotAchievement;
        _asteroidsGenerator.Start();

        _spaceshipsGenerator = new SpaceshipsGenerator(spaceshipsContainer, playerTransform, spaceshipsInitialCount);
        _spaceshipsGenerator.OnGotAchievement += OnGotAchievement;
    }

    private void CreateWeapon(Transform playerTransform, Transform weaponTransform)
    {
        _machineGunController = new MachineGunController(machineGunContainer, weaponTransform, machineGunAmmunitionCount);
        _laserController = new LaserController(laserContainer, weaponTransform, playerTransform, laserAmmunitionInitialCount, laserOneShotRefillTime);
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
