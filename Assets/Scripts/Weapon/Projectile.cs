using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxLifeTime = 5f;

        public Action OnProjectileUpdate;
        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;

        public void Update()
        {
            OnProjectileUpdate?.Invoke();
        }
    }
}
