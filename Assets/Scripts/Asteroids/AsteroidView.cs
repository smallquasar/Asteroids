using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D asteroidCollider;
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float maxLifeTime = 30f;

        public Action OnAsteroidUpdate;
        public Action<AsteroidDisappearingType> OnAsteroidDestroy;

        public AsteroidType AsteroidType { get; set; }

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

        public void SetAsteroidImage(Sprite sprite)
        {
            if (sprite == null)
                return;

            spriteRenderer.sprite = sprite;
            asteroidCollider.size = spriteRenderer.bounds.size;
        }

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
