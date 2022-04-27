using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts
{
    public class StatisticsCollector
    {
        private PlayerController _playerController;
        private LaserController _laserController;

        public StatisticsCollector(PlayerController playerController, LaserController laserController)
        {
            _playerController = playerController;
            _laserController = laserController;
        }

        public PlayerStatistics GetStatistics()
        {
            Vector2 playerCoords = _playerController.Coordinates;
            float playerVelocity = _playerController.Velocity;
            float playerAngle = _playerController.Angle;
            int laserCount = _laserController.AmmunitionCurrentCount;
            int cooldown = 3; //HARDCODE!

            return new PlayerStatistics()
            {
                Coordinates = playerCoords,
                Velocity = playerVelocity,
                Angle = Mathf.RoundToInt(playerAngle),
                LaserAmmunitionCount = laserCount,
                LaserCooldown = cooldown
            };
        }
    }
}
