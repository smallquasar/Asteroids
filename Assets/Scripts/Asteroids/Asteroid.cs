using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float maxLifeTime = 30f;

        public Action OnAsteroidUpdate;
        public Action<AsteroidDisappearingType> OnAsteroidDestroy;

        public AsteroidType AsteroidType { get; set; }

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

        public void Update()
        {
            OnAsteroidUpdate?.Invoke();
        }

        public void DestroyAsteroid(AsteroidDisappearingType asteroidDisappearingType)
        {
            OnAsteroidDestroy?.Invoke(asteroidDisappearingType);
        }
    }
}
