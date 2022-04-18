using Assets.Scripts.Generation;
using Assets.Scripts.Player;

namespace Assets.Scripts.Weapon
{
    public class ProjectileCreator : IPoolObjectCreator<ProjectileController>
    {
        public ProjectileController Create()
        {
            return new ProjectileController();
        }
    }
}
