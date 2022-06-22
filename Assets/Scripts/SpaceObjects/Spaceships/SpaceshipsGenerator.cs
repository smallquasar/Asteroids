using Assets.Scripts.Events;
using Assets.Scripts.Events.SpaceEventArgs;
using Assets.Scripts.Generation;
using Assets.Scripts.PlayerInfo;
using Assets.Scripts.SpaceObjectsInfo;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipsGenerator
    {
        private EventNotifier _eventNotifier;

        private Pool<SpaceshipController> _spaceshipsPool;

        public SpaceshipsGenerator(Transform spaceshipsContainer, Transform playerTransform, int initialCount, SpaceObjectVariants spaceshipVariants,
            EventNotifier eventNotifier)
        {
            _eventNotifier = eventNotifier;

            SpaceshipCreator creator = new SpaceshipCreator(playerTransform, spaceshipsContainer, spaceshipVariants, eventNotifier);
            _spaceshipsPool = new Pool<SpaceshipController>(creator, initialCount, canExpandPool: true);
        }

        public void SpawnNewShip()
        {
            SpaceshipController spaceship = _spaceshipsPool.Get();
            spaceship.OnDestroy += DestroySpaceship;
            spaceship?.Init(GenerationUtils.GenerateLocation(isInitSpawn: false));
            spaceship.SetActive(true);
        }

        private void DestroySpaceship(SpaceshipController spaceship, bool isDestroyedByPlayer)
        {
            _spaceshipsPool.ReturnToPool(spaceship);

            spaceship.OnDestroy -= DestroySpaceship;

            if (isDestroyedByPlayer)
            {
                _eventNotifier.Notify(Events.EventType.GotAchievement, new AchievementEventArgs(Achievement.DestroySpaceShip));
            }
        }
    }
}
