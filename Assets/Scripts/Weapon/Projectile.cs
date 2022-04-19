using System;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D projectileCollider;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxLifeTime = 5f;

        public Action OnProjectileUpdate;
        public Action<Collider2D> OnProjectileCrossObject;

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

        public void SetProjectileImage(Sprite sprite)
        {
            if (sprite == null)
                return;

            spriteRenderer.sprite = sprite;
            projectileCollider.size = spriteRenderer.bounds.size;
        }

        public void Update()
        {
            OnProjectileUpdate?.Invoke();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            OnProjectileCrossObject?.Invoke(collision);
        }
    }
}
