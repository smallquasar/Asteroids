using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;
using Assets.Scripts.Player;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelInfo
{
    public class Level : IObserver
    {
        public LevelSettings LevelSettings { get; set; }
        public AsteroidsGenerator AsteroidsGenerator { get; set; }
        public SpaceshipsGenerator SpaceshipsGenerator { get; set; }
        public PlayerController PlayerController { get; set; }
        public MachineGunController MachineGunController { get; set; }
        public LaserController LaserController { get; set; }

        private bool _isLevelStarted = false;

        private float _spawnAsteroidTimeLeft = 0;
        private float _spawnSpaceshipTimeLeft = 0;

        public void StartLevel()
        {
            _spawnAsteroidTimeLeft = LevelSettings.AsteroidAppearanceTime;
            _spawnSpaceshipTimeLeft = GetTimeForSpaceshipWait();
            _isLevelStarted = true;
        }

        public void Update(Events.EventType eventType, System.EventArgs param)
        {
            if (!_isLevelStarted || eventType != Events.EventType.SpawnObjects)
            {
                return;
            }

            _spawnAsteroidTimeLeft -= Time.deltaTime;
            if (_spawnAsteroidTimeLeft < 0)
            {
                AsteroidsGenerator.SpawnNewAsteroid(isInitSpawn: false);
                _spawnAsteroidTimeLeft = LevelSettings.AsteroidAppearanceTime;
            }

            _spawnSpaceshipTimeLeft -= Time.deltaTime;
            if (_spawnSpaceshipTimeLeft < 0)
            {
                SpaceshipsGenerator.SpawnNewShip();
                _spawnSpaceshipTimeLeft = GetTimeForSpaceshipWait();
            }
        }

        private float GetTimeForSpaceshipWait()
        {
            float timeForWait = Random.Range(LevelSettings.SpaceshipAppearanceTimeFrom, LevelSettings.SpaceshipAppearanceTimeTo + 1);
            return timeForWait;
        }
    }
}
