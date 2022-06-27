using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Player;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
using System.Collections.Generic;
using UnityEngine;
using EventType = Assets.Scripts.Events.EventType;

namespace Assets.Scripts.LevelInfo
{
    public class LevelBuilder
    {
        private Level _level;        
        private EventNotifier _eventNotifier;

        private LevelSettings _levelSettings;
        private PlayerController _playerController;
        private MachineGunController _machineGunController;
        private LaserController _laserController;

        public LevelBuilder(EventNotifier eventNotifier)
        {
            _level = new Level();
            _eventNotifier = eventNotifier;
        }

        public void Reset()
        {
            _level = new Level();
            _levelSettings = null;

            _eventNotifier.Detach(new List<IObserver> { _playerController, _machineGunController, _laserController });
            _playerController = null;
            _machineGunController = null;
            _laserController = null;
        }

        public void SetLevelSettings(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;

            _level.LevelSettings = _levelSettings;
        }

        public void CreatePlayer(float worldHeight, float worldWidth, GameObject player, PlayerInput playerInput)
        {
            _playerController = new PlayerController(player, playerInput, worldHeight, worldWidth, _eventNotifier);
            _playerController.SetPlayerPosition(_levelSettings.PlayerStartPosition, _levelSettings.PlayerStartRotation);

            _eventNotifier.Attach(_playerController, EventType.Update);

            _level.PlayerController = _playerController;
        }

        public void CreateSpaceObjectGenerators(Transform wholeAsteroidsContainer, Transform asteroidFragmentsContainer, Transform spaceshipsContainer)
        {
            _level.AsteroidsGenerator = new AsteroidsGenerator(wholeAsteroidsContainer, asteroidFragmentsContainer,
                _levelSettings.AsteroidsInitialCount, _levelSettings.AsteroidVariants, _eventNotifier);
            _level.SpaceshipsGenerator = new SpaceshipsGenerator(spaceshipsContainer, _playerController.PlayerTransform,
                _levelSettings.SpaceshipsInitialCount, _levelSettings.SpaceshipVariants, _eventNotifier);
        }

        public void CreateWeapon(Transform machineGunContainer, Transform laserContainer)
        {
            _machineGunController = new MachineGunController(machineGunContainer, _playerController.WeaponTransform,
                _levelSettings.MachineGunAmmunitionInitialCount, _levelSettings.WeaponTypes, _eventNotifier);
            _laserController = new LaserController(laserContainer, _playerController.WeaponTransform, _playerController.PlayerTransform,
                _levelSettings.LaserAmmunitionInitialCount, _levelSettings.LaserOneShotRefillTime, _levelSettings.WeaponTypes, _eventNotifier);

            _level.MachineGunController = _machineGunController;
            _level.LaserController = _laserController;

            _eventNotifier.Attach(_machineGunController, EventType.WeaponShot);
            _eventNotifier.Attach(_laserController, EventType.WeaponShot);
            _eventNotifier.Attach(_laserController, EventType.Update);
        }

        public Level GetResult()
        {
            return _level;
        }
    }
}
