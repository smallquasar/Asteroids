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
        private EventNotifier _eventNotifier;

        public AsteroidCreator(AsteroidType type, Transform parentContainer, List<AsteroidVariants> asteroidVariants, EventNotifier eventNotifier)
        {
            _asteroidType = type;
            _parentContainer = parentContainer;
            _asteroidInfo = asteroidVariants.FirstOrDefault(x => x.AsteroidType == _asteroidType);
            _eventNotifier = eventNotifier;
        }

        public AsteroidController Create()
        {
            AsteroidController newAsteroid = new AsteroidController(_asteroidType, GetPrefab(), _parentContainer);
            _eventNotifier.Attach(newAsteroid, Events.EventType.Update);
            _eventNotifier.Attach(newAsteroid, Events.EventType.Destroy);

            return newAsteroid;
        }

        private GameObject GetPrefab()
        {
            return _asteroidInfo?.GetRandomVariant() ?? null;
        }
    }
}
