using Assets.Scripts.SpaceObjectsInfo;
using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidVariants_", menuName = "ScriptableObjects/AsteroidVariants")]
    public class AsteroidVariants : SpaceObjectVariants
    {
        [SerializeField] private AsteroidType asteroidType;

        public AsteroidType AsteroidType => asteroidType;
    }
}
