using Assets.Scripts.Generation;
using Assets.Scripts.PlayerInfo;
using System;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipsGenerator
    {
        public Action<Achievement> OnGotAchievement;

        private Pool<SpaceshipController> _spaceshipsPool;

        public SpaceshipsGenerator(Transform spaceshipsContainer, Transform playerTransform, int initialCount)
        {
            SpaceshipCreator creator = new SpaceshipCreator(playerTransform, spaceshipsContainer);
            _spaceshipsPool = new Pool<SpaceshipController>(creator, initialCount, canExpandPool: true);
        }

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
