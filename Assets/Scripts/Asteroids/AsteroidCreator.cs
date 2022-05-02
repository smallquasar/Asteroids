using Assets.Scripts.Generation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidCreator : IPoolObjectCreator<AsteroidController>
    {
        private AsteroidType _asteroidType;
        private Transform _parentContainer;
        private AsteroidVariants _asteroidInfo;

        public AsteroidCreator(AsteroidType type, Transform parentContainer, List<AsteroidVariants> asteroidVariants)
        {
            _asteroidType = type;
            _parentContainer = parentContainer;
            _asteroidInfo = asteroidVariants.FirstOrDefault(x => x.AsteroidType == _asteroidType);
        }

        public AsteroidController Create()
        {
            return new AsteroidController(_asteroidType, GetPrefab(), _parentContainer);
        }

        private GameObject GetPrefab()
        {
            return _asteroidInfo?.GetRandomVariant() ?? null;
        }
    }
}
