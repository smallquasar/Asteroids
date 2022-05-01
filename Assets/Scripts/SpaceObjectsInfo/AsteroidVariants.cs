using Assets.Scripts.Asteroids;
using UnityEngine;

namespace Assets.Scripts.SpaceObjectsInfo
{
    [CreateAssetMenu(fileName = "AsteroidVariants_", menuName = "ScriptableObjects/AsteroidVariants")]
    public class AsteroidVariants : SpaceObjectVariants
    {
        [SerializeField] private AsteroidType asteroidType;

        public AsteroidType AsteroidType => asteroidType;
    }
}
