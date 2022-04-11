using Assets.Scripts;
using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidsContainer;
    [SerializeField] int asteroidsInitialCount = 5;
    
    [SerializeField] private List<SpawnZones> _spawnZones;

    private AsteroidsGenerator _asteroidsGenerator;
    private PlayerController _playerController;

    public void Start()
    {
        GenerationUtils.SetSpawnZones(_spawnZones);
        _asteroidsGenerator = new AsteroidsGenerator(asteroidPrefab, asteroidsContainer, asteroidsInitialCount);
        _asteroidsGenerator.Start();

        Camera mainCamera = UnityEngine.Camera.main;
        float worldHeight = mainCamera.orthographicSize * 2;
        float worldWidth = worldHeight * mainCamera.aspect;

        GameObject player = Instantiate(playerPrefab);
        _playerController = new PlayerController(player, worldHeight, worldWidth);

        StartCoroutine(Spawn());
    }

    public void Update()
    {
        
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            _asteroidsGenerator.SpawnNew();
            yield return new WaitForSeconds(5);
        }
    }
}
