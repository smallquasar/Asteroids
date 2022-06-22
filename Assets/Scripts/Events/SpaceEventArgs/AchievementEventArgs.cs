using Assets.Scripts.PlayerInfo;
using System;

namespace Assets.Scripts.Events.SpaceEventArgs
{
    public class AchievementEventArgs : EventArgs
    {
        public readonly Achievement Achievement;

        public AchievementEventArgs(Achievement achievement)
        {
            Achievement = achievement;
        }
    }
}
