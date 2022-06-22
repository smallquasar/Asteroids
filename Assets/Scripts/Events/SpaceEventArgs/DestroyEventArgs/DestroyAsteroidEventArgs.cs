using Assets.Scripts.Asteroids;

namespace Assets.Scripts.Events.SpaceEventArgs
{
    public class DestroyAsteroidEventArgs : DestroyEventArgs
    {
        public readonly AsteroidDisappearingType AsteroidDisappearingType;

        public DestroyAsteroidEventArgs(int objectToDestroyId, AsteroidDisappearingType asteroidDisappearingType)
            :base(objectToDestroyId)
        {
            AsteroidDisappearingType = asteroidDisappearingType;
        }
    }
}
