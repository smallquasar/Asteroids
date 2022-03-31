using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidController
    {
        private GameObject _asteroid;
        private AsteroidView _asteroidView;

        public AsteroidController(GameObject asteroid)
        {
            _asteroid = asteroid;
            //_asteroidView = _asteroid.GetComponent<AsteroidView>();
        }

        public void Init()
        {
            _asteroid.transform.position = GenerationUtils.GenerateLocation();
        }

        public void SetActive(bool isActive)
        {
            _asteroid.SetActive(isActive);
        }
    }
}
