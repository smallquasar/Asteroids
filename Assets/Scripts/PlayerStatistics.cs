using UnityEngine;

namespace Assets.Scripts
{
    public struct PlayerStatistics
    {
        public Vector2 Coordinates { get; set; }
        public int Angle { get; set; }
        public float Velocity { get; set; }
        public int LaserAmmunitionCount { get; set; }
        public int LaserCooldown { get; set; }
    }
}
