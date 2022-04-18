using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D asteroidCollider;

        public Action OnAsteroidUpdate;
        public Action OnAsteroidDestroy;

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

        public void DestroyAsteroid()
        {
            OnAsteroidDestroy?.Invoke();
        }
    }
}
