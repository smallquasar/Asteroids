using UnityEngine;

namespace Assets.Scripts.Spaceships
{
    public class Spaceship : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxLifeTime = 50f;

        public float Speed => speed;
        public float MaxLifeTime => maxLifeTime;
    }
}
