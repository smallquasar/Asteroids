using UnityEngine;

namespace Assets.Scripts.PlayerInfo
{
    public struct PlayerStatistics
    {
        public Vector2 Coordinates { get; set; }
        public int Angle { get; set; }
        public float Velocity { get; set; }
        public int LaserAmmunitionCount { get; set; }
        public float LaserCooldown { get; set; }
    }
}
