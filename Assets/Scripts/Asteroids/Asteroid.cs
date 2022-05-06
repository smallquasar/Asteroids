using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float maxLifeTime = 30f;

        public AsteroidType AsteroidType { get; set; }

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;
    }
}
