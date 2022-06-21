using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Player;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
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

        public LevelBuilder(EventNotifier eventNotifier)
        {
            _level = new Level();
            _eventNotifier = eventNotifier;
        }

        public void Reset()
        {
            _level = new Level();

            _levelSettings = null;
            if (_playerController != null)
            {
                _eventNotifier.Detach(_playerController);
                _playerController = null;
            }
        }

        public void SetLevelSettings(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;

            _level.LevelSettings = _levelSettings;
        }

        public void CreatePlayer(float worldHeight, float worldWidth, GameObject player, PlayerInput playerInput)
        {
            _playerController = new PlayerController(player, playerInput, worldHeight, worldWidth);
            _eventNotifier.Attach(_playerController, EventType.Update);
            _playerController.SetPlayerPosition(_levelSettings.PlayerStartPosition, _levelSettings.PlayerStartRotation);

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
            _level.MachineGunController = new MachineGunController(machineGunContainer, _playerController.WeaponTransform,
                _levelSettings.MachineGunAmmunitionInitialCount, _levelSettings.WeaponTypes, _eventNotifier);
            _level.LaserController = new LaserController(laserContainer, _playerController.WeaponTransform, _playerController.PlayerTransform,
                _levelSettings.LaserAmmunitionInitialCount, _levelSettings.LaserOneShotRefillTime, _levelSettings.WeaponTypes, _eventNotifier);
        }

        public Level GetResult()
        {
            return _level;
        }
    }
}
