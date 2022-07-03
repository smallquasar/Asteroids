using Assets.Scripts.LevelInfo;
using Assets.Scripts.Player;
using Assets.Scripts.Weapon;
using UnityEngine;

namespace Assets.Scripts.PlayerInfo
{
    public class PlayerStatistics
    {
        public Vector2 Coordinates { get; private set; }
        public int Angle { get; private set; }
        public float Velocity { get; private set; }
        public int LaserAmmunitionCount { get; private set; }
        public float LaserCooldown { get; private set; }

        private PlayerController _playerController;
        private LaserController _laserController;

        public PlayerStatistics(Level level)
        {
            _playerController = level.PlayerController;
            _laserController = level.LaserController;
        }

        public void UpdateStatistics()
        {
            Coordinates = _playerController.GetPlayerPositionWithOffset();
            Velocity = Mathf.Abs(_playerController.Velocity);
            Angle = Mathf.RoundToInt(_playerController.Angle);
            LaserAmmunitionCount = _laserController.AmmunitionCurrentCount;
            LaserCooldown = _laserController.LaserAmmunitionRefillTimeCounterCalculate();
        }
    }
}
