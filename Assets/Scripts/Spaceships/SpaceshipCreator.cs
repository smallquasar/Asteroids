using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipCreator : IPoolObjectCreator<SpaceshipController>
    {
        private Transform _playerTransform;
        private GameObject _prefab;
        private Transform _parentContainer;

        public SpaceshipCreator(Transform playerTransform, Transform parentContainer)
        {
            _playerTransform = playerTransform;
            _prefab = GetPrefab();
            _parentContainer = parentContainer;
        }

        public SpaceshipController Create()
        {
            return new SpaceshipController(_playerTransform, _prefab, _parentContainer);
        }

        private GameObject GetPrefab()
        {
            return GameData.GetSpaceshipPrefab();
        }
    }
}
