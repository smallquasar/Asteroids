using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float rotationSpeed = 140f;
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float deceleration = 4f;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Action OnPlayerUpdate;
        public Action<Collider2D> OnPlayerCrossObject;

        public float MaxSpeed => maxSpeed;
        public float RotationSpeed => rotationSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public Transform WeaponTransform => weaponTransform;
        public SpriteRenderer SpriteRenderer => spriteRenderer;

        public void Update()
        {
            OnPlayerUpdate?.Invoke();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            OnPlayerCrossObject?.Invoke(collision);
        }
    }
}
