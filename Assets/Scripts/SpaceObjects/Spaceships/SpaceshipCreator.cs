using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using Assets.Scripts.SpaceObjectsInfo;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipCreator : IPoolObjectCreator<SpaceshipController>
    {
        private Transform _playerTransform;
        private Transform _parentContainer;
        private SpaceObjectVariants _spaceshipVariants;
        private EventNotifier _eventNotifier;

        public SpaceshipCreator(Transform playerTransform, Transform parentContainer, SpaceObjectVariants spaceshipVariants, EventNotifier eventNotifier)
        {
            _playerTransform = playerTransform;
            _parentContainer = parentContainer;
            _spaceshipVariants = spaceshipVariants;
            _eventNotifier = eventNotifier;
        }

        public SpaceshipController Create()
        {
            SpaceshipController newSpaceship = new SpaceshipController(_playerTransform, GetPrefab(), _parentContainer);
            _eventNotifier.Attach(newSpaceship, Events.EventType.Update);
            _eventNotifier.Attach(newSpaceship, Events.EventType.Destroy);

            return newSpaceship;
        }

        private GameObject GetPrefab()
        {
            return _spaceshipVariants?.GetRandomVariant() ?? null;
        }
    }
}
