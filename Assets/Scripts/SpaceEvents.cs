using Assets.Scripts.Asteroids;
using Assets.Scripts.Events;

namespace Assets.Scripts.Main
{
    public struct SpaceEvents
    {
        public EventManager UpdateEventManager { get; set; }
        public DestroyEventManager DestroySpaceshipsEventManager { get; set; }
        public DestroyEventManagerWithParameters<AsteroidDisappearingType> DestroyAsteroidsEventManager { get; set; }
    }
}
