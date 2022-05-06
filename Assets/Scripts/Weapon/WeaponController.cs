using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Generation;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class WeaponController
    {
        protected Transform _prefabContainer;
        protected Transform _weaponPosition;

        protected Pool<ProjectileController> _projectilesPool;

        protected DestroyEventManager _destroyEventManager;
        protected DestroyEventManagerWithParameters<AsteroidDisappearingType> _destroyEventManagerWithParameters;

        public WeaponController(Transform prefabContainer, Transform weaponPosition, DestroyEventManager destroyEventManager,
            DestroyEventManagerWithParameters<AsteroidDisappearingType> destroyEventManagerWithParameters)
        {
            _prefabContainer = prefabContainer;
            _weaponPosition = weaponPosition;
            _destroyEventManager = destroyEventManager;
            _destroyEventManagerWithParameters = destroyEventManagerWithParameters;
        }

        public abstract void OnWeaponShot(Vector3 direction);

        protected virtual void DestroySpaceship(int spaceshipId)
        {
            List<int> spaceshipsToDestroy = new List<int>();
            spaceshipsToDestroy.Add(spaceshipId);

            _destroyEventManager.SetCurrentObserversIdForNotify(spaceshipsToDestroy);
            _destroyEventManager.Notify();
        }

        protected virtual void DestroyAsteroid(int asteroidId, AsteroidDisappearingType asteroidDisappearingType)
        {
            List<int> asteroidsToDestroy = new List<int>();
            asteroidsToDestroy.Add(asteroidId);

            _destroyEventManagerWithParameters.SetCurrentObserversIdForNotify(asteroidsToDestroy);
            _destroyEventManagerWithParameters.SetParameter(asteroidDisappearingType);
            _destroyEventManagerWithParameters.Notify();
        }
    }
}
