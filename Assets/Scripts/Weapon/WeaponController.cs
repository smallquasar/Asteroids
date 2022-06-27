using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Events.SpaceEventArgs;
using Assets.Scripts.Generation;
using System;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class WeaponController : IObserver
    {
        protected Transform _prefabContainer;
        protected Transform _weaponPosition;

        protected Pool<ProjectileController> _projectilesPool;

        protected EventNotifier _eventNotifier;

        public WeaponController(Transform prefabContainer, Transform weaponPosition, EventNotifier eventNotifier)
        {
            _prefabContainer = prefabContainer;
            _weaponPosition = weaponPosition;
            _eventNotifier = eventNotifier;
        }

        public abstract void Update(Events.EventType eventType, EventArgs param);

        public abstract void OnWeaponShot(Vector3 direction);

        protected virtual void DestroySpaceship(int spaceshipId)
        {
            _eventNotifier.Notify(Events.EventType.Destroy, new DestroyEventArgs(spaceshipId));
        }

        protected virtual void DestroyAsteroid(int asteroidId, AsteroidDisappearingType asteroidDisappearingType)
        {
            _eventNotifier.Notify(Events.EventType.Destroy, new DestroyAsteroidEventArgs(asteroidId, asteroidDisappearingType));
        }        
    }
}
