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

        public SpaceshipCreator(Transform playerTransform, Transform parentContainer, SpaceObjectVariants spaceshipVariants)
        {
            _playerTransform = playerTransform;
            _parentContainer = parentContainer;
            _spaceshipVariants = spaceshipVariants;
        }

        public SpaceshipController Create()
        {
            return new SpaceshipController(_playerTransform, GetPrefab(), _parentContainer);
        }

        private GameObject GetPrefab()
        {
            return _spaceshipVariants?.GetRandomVariant() ?? null;
        }
    }
}
