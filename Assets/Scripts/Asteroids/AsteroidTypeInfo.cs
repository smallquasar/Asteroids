using UnityEngine;

namespace Assets.Scripts.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidType_", menuName = "ScriptableObjects/AsteroidType")]
    public class AsteroidTypeInfo : ScriptableObject
    {
        [SerializeField] private Sprite asteroidSprite;
        [SerializeField] private AsteroidType asteroidType;

        public Sprite AsteroidSprite => asteroidSprite;
        public AsteroidType AsteroidType => asteroidType;
    }
}
