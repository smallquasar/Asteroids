using Assets.Scripts.Events;
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
        private EventManager _eventManager;
        private DestroyEventManagerWithParameters<AsteroidDisappearingType> _destroyEventManagerWithParameters;

        public AsteroidCreator(AsteroidType type, Transform parentContainer, List<AsteroidVariants> asteroidVariants, EventManager eventManager,
            DestroyEventManagerWithParameters<AsteroidDisappearingType> destroyEventManagerWithParameters)
        {
            _asteroidType = type;
            _parentContainer = parentContainer;
            _asteroidInfo = asteroidVariants.FirstOrDefault(x => x.AsteroidType == _asteroidType);
            _eventManager = eventManager;
            _destroyEventManagerWithParameters = destroyEventManagerWithParameters;
        }

        public AsteroidController Create()
        {
            AsteroidController newAsteroid = new AsteroidController(_asteroidType, GetPrefab(), _parentContainer);
            _eventManager.Attach(newAsteroid);
            _destroyEventManagerWithParameters.Attach(newAsteroid, newAsteroid.GetId());

            return newAsteroid;
        }

        private GameObject GetPrefab()
        {
            return _asteroidInfo?.GetRandomVariant() ?? null;
        }
    }
}
