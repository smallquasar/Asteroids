using Assets.Scripts.Asteroids;
using Assets.Scripts.Player;
using Assets.Scripts.Spaceships;
using Assets.Scripts.Weapon;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelInfo
{
    public class Level
    {
        public LevelSettings LevelSettings { get; set; }
        public AsteroidsGenerator AsteroidsGenerator { get; set; }
        public SpaceshipsGenerator SpaceshipsGenerator { get; set; }
        public PlayerController PlayerController { get; set; }
        public MachineGunController MachineGunController { get; set; }
        public LaserController LaserController { get; set; }

        public IEnumerator SpawnAsteroids()
        {
            while (true)
            {
                AsteroidsGenerator.SpawnNewAsteroid(isInitSpawn: false);
                yield return new WaitForSeconds(LevelSettings.AsteroidAppearanceTime);
            }
        }

        public IEnumerator SpawnSpaceships()
        {
            while (true)
            {
                int timeForWait = Random.Range(LevelSettings.SpaceshipAppearanceTimeFrom, LevelSettings.SpaceshipAppearanceTimeTo + 1);
                yield return new WaitForSeconds(timeForWait);

                SpaceshipsGenerator.SpawnNewShip();
            }
        }
    }
}
