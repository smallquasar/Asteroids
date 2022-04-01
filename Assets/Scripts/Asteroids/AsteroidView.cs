using System;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidView : MonoBehaviour
    {
        public Action OnAsteroidUpdate;

        public void Update()
        {
            OnAsteroidUpdate?.Invoke();
        }
    }
}
