using System;
using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxLifeTime = 30f;

        public Action OnSpaceshipUpdate;
        public Action OnSpaceshipDestroy;

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

        public void Update()
        {
            OnSpaceshipUpdate?.Invoke();
        }

        public void DestroySpaceship()
        {
            OnSpaceshipDestroy?.Invoke();
        }
    }
}
