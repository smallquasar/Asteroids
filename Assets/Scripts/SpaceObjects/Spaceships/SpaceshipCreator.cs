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
        private EventManager _updateEventManager;
        private DestroyEventManager _destroyEventManager;

        public SpaceshipCreator(Transform playerTransform, Transform parentContainer, SpaceObjectVariants spaceshipVariants, EventManager updateEventManager,
            DestroyEventManager destroyEventManager)
        {
            _playerTransform = playerTransform;
            _parentContainer = parentContainer;
            _spaceshipVariants = spaceshipVariants;
            _updateEventManager = updateEventManager;
            _destroyEventManager = destroyEventManager;
        }

        public SpaceshipController Create()
        {
            SpaceshipController newSpaceship = new SpaceshipController(_playerTransform, GetPrefab(), _parentContainer);
            _updateEventManager.Attach(newSpaceship);
            _destroyEventManager.Attach(newSpaceship, newSpaceship.GetId());

            return newSpaceship;
        }

        private GameObject GetPrefab()
        {
            return _spaceshipVariants?.GetRandomVariant() ?? null;
        }
    }
}
