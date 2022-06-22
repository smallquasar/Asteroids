using Assets.Scripts.Events;
using Assets.Scripts.Events.SpaceEventArgs;
using Assets.Scripts.Generation;
using Assets.Scripts.PlayerInfo;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidsGenerator
    {
        private EventNotifier _eventNotifier;

        private int _initialCount;

        private Pool<AsteroidController> _asteroidsPool;
        private Pool<AsteroidController> _asteroidFragmentsPool;

        public AsteroidsGenerator(Transform asteroidsContainer, Transform fragmentsContainer, int initialCount, List<AsteroidVariants> asteroidVariants,
            EventNotifier eventNotifier)
        {
            _initialCount = initialCount;
            _eventNotifier = eventNotifier;

            AsteroidCreator asteroidsCreator = new AsteroidCreator(AsteroidType.Asteroid, asteroidsContainer, asteroidVariants, eventNotifier);
            AsteroidCreator asteroidFragmentsCreator = new AsteroidCreator(AsteroidType.AsteroidFragment, fragmentsContainer, asteroidVariants, eventNotifier);

            _asteroidsPool = new Pool<AsteroidController>(asteroidsCreator, _initialCount, canExpandPool: true);
            _asteroidFragmentsPool = new Pool<AsteroidController>(asteroidFragmentsCreator, _initialCount * 2, canExpandPool: true);
        }

        public void Start()
        {
            SpawnInitialAsteroidsCount();
        }

        public void SpawnInitialAsteroidsCount()
        {
            for (int i = 0; i < _initialCount; i++)
            {
                SpawnNewAsteroid(isInitSpawn: true);
            }
        }

        public void SpawnNewAsteroid(bool isInitSpawn)
        {
            SpawnNew(_asteroidsPool, GenerationUtils.GenerateLocation(isInitSpawn));
        }

        private void DestroyAsteroid(AsteroidController asteroid, AsteroidDisappearingType asteroidDisappearingType)
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
                _eventNotifier.Notify(Events.EventType.GotAchievement, new AchievementEventArgs(achievement));
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
