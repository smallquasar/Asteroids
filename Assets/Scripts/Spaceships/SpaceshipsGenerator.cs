using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipsGenerator
    {
        public Action<Achievement> OnGotAchievement;

        private GameObject _prefab;
        private Transform _spaceshipsContainer;
        private Transform _playerTransform;
        private int _initialCount;

        private Pool<SpaceshipController> _spaceshipsPool;

        public SpaceshipsGenerator(GameObject prefab, Transform spaceshipsContainer, Transform playerTransform, int initialCount)
        {
            _prefab = prefab;
            _spaceshipsContainer = spaceshipsContainer;
            _playerTransform = playerTransform;
            _initialCount = initialCount;
        }

        public void Start()
        {
            _spaceshipsPool =
                new Pool<SpaceshipController>(new SpaceshipCreator(_playerTransform), _prefab, _spaceshipsContainer, _initialCount, canExpandPool: true);

            //SpawnInitialSpaceshipsCount();
        }

        //public void SpawnInitialSpaceshipsCount()
        //{
        //    for (int i = 0; i < _initialCount; i++)
        //    {
        //        SpawnNewShip();
        //    }
        //}

        public void SpawnNewShip()
        {
            SpaceshipController spaceship = _spaceshipsPool.Get();
            spaceship.OnDestroy += DestroySpaceship;
            spaceship?.Init(GenerationUtils.GenerateLocation());
            spaceship.SetActive(true);
        }

        public void DestroySpaceship(SpaceshipController spaceship, bool isDestroyedByPlayer)
        {
            _spaceshipsPool.ReturnToPool(spaceship);

            spaceship.OnDestroy -= DestroySpaceship;

            if (isDestroyedByPlayer)
            {
                OnGotAchievement?.Invoke(Achievement.DestroySpaceShip);
            }
        }
    }
}
