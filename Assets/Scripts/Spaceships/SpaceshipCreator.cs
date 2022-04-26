using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class SpaceshipCreator : IPoolObjectCreator<SpaceshipController>
    {
        private Transform _playerTransform;

        public SpaceshipCreator(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public SpaceshipController Create()
        {
            return new SpaceshipController(_playerTransform);
        }
    }
}
