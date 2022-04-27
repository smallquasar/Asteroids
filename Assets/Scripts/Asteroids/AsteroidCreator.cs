using Assets.Scripts.Generation;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidCreator : IPoolObjectCreator<AsteroidController>
    {
        private AsteroidType _asteroidType;
        private GameObject _prefab;
        private Transform _parentContainer;

        public AsteroidCreator(AsteroidType type, GameObject prefab, Transform parentContainer)
        {
            _asteroidType = type;
            _prefab = prefab;
            _parentContainer = parentContainer;
        }

        public AsteroidController Create()
        {
            return new AsteroidController(_asteroidType, _prefab, _parentContainer);
        }
    }
}
