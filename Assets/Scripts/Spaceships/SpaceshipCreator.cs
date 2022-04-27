using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipCreator : IPoolObjectCreator<SpaceshipController>
    {
        private Transform _playerTransform;
        private GameObject _prefab;
        private Transform _parentContainer;

        public SpaceshipCreator(Transform playerTransform, GameObject prefab, Transform parentContainer)
        {
            _playerTransform = playerTransform;
            _prefab = prefab;
            _parentContainer = parentContainer;
        }

        public SpaceshipController Create()
        {
            return new SpaceshipController(_playerTransform, _prefab, _parentContainer);
        }
    }
}
