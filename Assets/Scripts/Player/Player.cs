using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 4f;
        [SerializeField] private float rotationSpeed = 80f;
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float deceleration = 3f;
        [SerializeField] private Transform weaponTransform;

        public Action OnPlayerUpdate;

        public float MaxSpeed => maxSpeed;
        public float RotationSpeed => rotationSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public Transform WeaponTransform => weaponTransform;

        public void Update()
        {
            OnPlayerUpdate?.Invoke();
        }
    }
}
