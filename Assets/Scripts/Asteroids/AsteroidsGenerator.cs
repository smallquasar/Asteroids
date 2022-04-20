using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts
{
    public class AsteroidsGenerator
    {
        private GameObject _prefab;
        private Transform _asteroidsContainer;
        private Transform _fragmentsContainer;
        private int _initialCount;

        private Pool<AsteroidController> _asteroidsPool;
        private Pool<AsteroidController> _asteroidFragmentsPool;

        public AsteroidsGenerator(GameObject prefab, Transform asteroidsContainer, Transform fragmentsContainer, int initialCount)
        {
            _prefab = prefab;
            _asteroidsContainer = asteroidsContainer;
            _fragmentsContainer = fragmentsContainer;
            _initialCount = initialCount;
        }

        public void Start()
        {
            _asteroidsPool =
                new Pool<AsteroidController>(new AsteroidCreator(AsteroidType.Asteroid), _prefab, _asteroidsContainer, _initialCount, canExpandPool: true);
            _asteroidFragmentsPool =
                new Pool<AsteroidController>(new AsteroidCreator(AsteroidType.AsteroidFragment), _prefab, _fragmentsContainer, _initialCount * 2, canExpandPool: true);

            for (int i = 0; i < _initialCount; i++)
            {
                SpawnNewAsteroid();
            }
        }

        public void SpawnNewAsteroid()
        {
            SpawnNew(_asteroidsPool, GenerationUtils.GenerateLocation());
        }

        public void DestroyAsteroid(AsteroidController asteroid, bool isTotallyDestroy)
        {
            if (asteroid.AsteroidType == AsteroidType.Asteroid)
            {
                _asteroidsPool.ReturnToPool(asteroid);
            }
            else if (asteroid.AsteroidType == AsteroidType.AsteroidFragment)
            {
                _asteroidFragmentsPool.ReturnToPool(asteroid);
            }
            
            asteroid.OnDestroy -= DestroyAsteroid;

            if (!isTotallyDestroy)
            {
                Vector3 asteroidPosition = asteroid.CurrentPosition;
                SpawnAsteroidFragments(asteroidPosition);
            }
        }

        private void SpawnNew(Pool<AsteroidController> pool, Vector3 initPosition)
        {
            AsteroidController asteroid = pool.Get();
            asteroid.OnDestroy += DestroyAsteroid;
            asteroid?.Init(initPosition);
            asteroid.SetActive(true);
        }       

        private void SpawnAsteroidFragments(Vector3 currentPosition)
        {
            SpawnNew(_asteroidFragmentsPool, currentPosition);
            SpawnNew(_asteroidFragmentsPool, currentPosition);
        }
    }
}
