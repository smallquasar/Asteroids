using Assets.Scripts.Generation;

namespace Assets.Scripts.Asteroids
{
    public class AsteroidCreator : IPoolObjectCreator<AsteroidController>
    {
        private AsteroidType _asteroidType;

        public AsteroidCreator(AsteroidType type)
        {
            _asteroidType = type;
        }

        public AsteroidController Create()
        {
            return new AsteroidController(_asteroidType);
        }
    }
}
