using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using Assets.Scripts.LevelInfo;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Assets.Scripts.Events.EventType;

public class GameManager : MonoBehaviour
{
    [Header("Asteroids")]
    [SerializeField] private Transform wholeAsteroidsContainer;
    [SerializeField] private Transform asteroidFragmentsContainer;

    [Header("Spaceships")]
    [SerializeField] private Transform spaceshipsContainer;

    [Header("Weapon")]
    [SerializeField] private Transform machineGunContainer;
    [SerializeField] private Transform laserContainer;

    [Header("UI")]
    [SerializeField] private GameUIView gameUIView;

    [Header("Game Data")]
    [SerializeField] private LevelSettings levelSettings;

    private Level _level;
    private LevelBuilder _levelBuilder;

    private PointsController _pointsController;
    private StatisticsCollector _statisticsCollector;

    private GameUIController _gameUIController;

    private PlayerInput _playerInput;

    private EventNotifier _eventNotifier;

    public void Awake()
    {
        _playerInput = new PlayerInput();
    }

    public void Start()
    {
        _eventNotifier = new EventNotifier();
        _levelBuilder = new LevelBuilder(_eventNotifier);

        GenerationUtils.SetSpawnZones(levelSettings.InitSpawnZones, levelSettings.SpawnZones);

        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        CreateLevel(worldHeight, worldWidth);        

        _level.AsteroidsGenerator.Start(); //fix

        _pointsController = new PointsController(levelSettings.DestroyPoints);
        _eventNotifier.Attach(_pointsController, EventType.GotAchievement);

        _statisticsCollector = new StatisticsCollector(_level.PlayerController, _level.LaserController);

        CreateGameUI();

        _level.PlayerController.OnDie += GameOver;        

        StartGame();
    }

    public void Update()
    {
        _eventNotifier.Notify(EventType.SpawnObjects, null);
        _eventNotifier.Notify(EventType.Update, null);

        _gameUIController.Update(_statisticsCollector.GetStatistics());
    }

    private void CreateLevel(float worldHeight, float worldWidth)
    {
        GameObject player = Instantiate(levelSettings.PlayerPrefab);

        _levelBuilder.SetLevelSettings(levelSettings);
        _levelBuilder.CreatePlayer(worldHeight, worldWidth, player, _playerInput);
        _levelBuilder.CreateSpaceObjectGenerators(wholeAsteroidsContainer, asteroidFragmentsContainer, spaceshipsContainer);
        _levelBuilder.CreateWeapon(machineGunContainer, laserContainer);

        _level = _levelBuilder.GetResult();
    }

    private void CreateGameUI()
    {
        _gameUIController = new GameUIController(gameUIView);
        _gameUIController.OnContinueGame += ContinueGame;
        _gameUIController.OnExitGame += ExitGame;
    }

    private void StartGame()
    {
        _level.StartLevel();      
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
