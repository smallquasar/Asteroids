using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class AsteroidsGenerator
    {
        public Action<Achievement> OnGotAchievement;

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

            SpawnInitialAsteroidsCount();
        }

        public void SpawnInitialAsteroidsCount()
        {
            for (int i = 0; i < _initialCount; i++)
            {
                SpawnNewAsteroid();
            }
        }

        public void SpawnNewAsteroid()
        {
            SpawnNew(_asteroidsPool, GenerationUtils.GenerateLocation());
        }

        public void DestroyAsteroid(AsteroidController asteroid, AsteroidDisappearingType asteroidDisappearingType)
        {
            AsteroidType asteroidType = asteroid.AsteroidType;
            if (asteroidType == AsteroidType.Asteroid)
            {
                _asteroidsPool.ReturnToPool(asteroid);
            }
            else if (asteroidType == AsteroidType.AsteroidFragment)
            {
                _asteroidFragmentsPool.ReturnToPool(asteroid);
            }
            
            asteroid.OnDestroy -= DestroyAsteroid;

            if (asteroidDisappearingType == AsteroidDisappearingType.Shaterred)
            {
                Vector3 asteroidPosition = asteroid.CurrentPosition;
                SpawnAsteroidFragments(asteroidPosition);
            }

            Achievement achievement = DefineAchievement(asteroidType, asteroidDisappearingType);
            if (achievement != Achievement.None)
            {
                OnGotAchievement?.Invoke(achievement);
            }
        }

        private Achievement DefineAchievement(AsteroidType asteroidType, AsteroidDisappearingType asteroidDisappearingType)
        {
            if (asteroidDisappearingType == AsteroidDisappearingType.GotAway)
            {
                return Achievement.None;
            }

            if (asteroidType == AsteroidType.Asteroid)
            {
                return asteroidDisappearingType == AsteroidDisappearingType.TotallyDestroyed
                    ? Achievement.DestroyAsteroid
                    : Achievement.BreakAsteroid;
            }

            if (asteroidType == AsteroidType.AsteroidFragment)
            {
                return Achievement.DestroyAsteroidFragment;
            }

            return Achievement.None;
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
