using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidsContainer;
    [SerializeField] int asteroidsInitialCount = 20;
    
    [SerializeField] private List<SpawnZones> _spawnZones;

    private AsteroidsGenerator asteroidsGenerator;

    public void Start()
    {
        GenerationUtils.SetSpawnZones(_spawnZones);
        asteroidsGenerator = new AsteroidsGenerator(asteroidPrefab, asteroidsContainer, asteroidsInitialCount);
        asteroidsGenerator.Start();
    }

    public void Update()
    {
        
    }
}
