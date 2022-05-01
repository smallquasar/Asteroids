using System;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxLifeTime = 5f;

        public Action OnProjectileUpdate;
        public Action<Collider2D> OnProjectileCrossObject;

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

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
