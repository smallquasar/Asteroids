using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;

namespace Assets.Scripts.SpaceObjects
{
    public class DestroySpaceObjectEvents
    {
        public DestroyEventManager DestroySpaceshipsEventManager { get; set; }
        public DestroyEventManagerWithParameters<AsteroidDisappearingType> DestroyAsteroidsEventManager { get; set; }

        public DestroySpaceObjectEvents()
        {
            DestroySpaceshipsEventManager = new DestroyEventManager();
            DestroyAsteroidsEventManager = new DestroyEventManagerWithParameters<AsteroidDisappearingType>();
        }
    }
}
