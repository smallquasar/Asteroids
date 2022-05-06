using Assets.Scripts.Asteroids;
using Assets.Scripts.Generation;
using Assets.Scripts.SpaceObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class WeaponController
    {
        protected Transform _prefabContainer;
        protected Transform _weaponPosition;

        protected Pool<ProjectileController> _projectilesPool;

        protected DestroySpaceObjectEvents _destroySpaceObjectEvents;

        public WeaponController(Transform prefabContainer, Transform weaponPosition, DestroySpaceObjectEvents destroySpaceObjectEvents)
        {
            _prefabContainer = prefabContainer;
            _weaponPosition = weaponPosition;
            _destroySpaceObjectEvents = destroySpaceObjectEvents;
        }

        public abstract void OnWeaponShot(Vector3 direction);

        protected virtual void DestroySpaceship(int spaceshipId)
        {
            List<int> spaceshipsToDestroy = new List<int>();
            spaceshipsToDestroy.Add(spaceshipId);

            var destroySpaceshipsEventManager = _destroySpaceObjectEvents.DestroySpaceshipsEventManager;
            destroySpaceshipsEventManager.SetCurrentObserversIdForNotify(spaceshipsToDestroy);
            destroySpaceshipsEventManager.Notify();
        }

        protected virtual void DestroyAsteroid(int asteroidId, AsteroidDisappearingType asteroidDisappearingType)
        {
            List<int> asteroidsToDestroy = new List<int>();
            asteroidsToDestroy.Add(asteroidId);

            var destroyAsteroidsEventManager = _destroySpaceObjectEvents.DestroyAsteroidsEventManager;
            destroyAsteroidsEventManager.SetCurrentObserversIdForNotify(asteroidsToDestroy);
            destroyAsteroidsEventManager.SetParameter(asteroidDisappearingType);
            destroyAsteroidsEventManager.Notify();
        }
    }
}
