using Assets.Scripts.Events;
using Assets.Scripts.Events.SpaceEventArgs;
using Assets.Scripts.PlayerInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LevelInfo
{
    public class PointsController : IObserver
    {
        public int Points { get; private set; }

        private List<DestroyPoints> _destroyPoints;

        public PointsController(List<DestroyPoints> destroyPoints)
        {
            _destroyPoints = destroyPoints;
        }

        public void Update(EventType eventType, EventArgs param)
        {
            if (eventType == EventType.GotAchievement)
            {
                AchievementEventArgs args = param as AchievementEventArgs;

                if (args != null)
                {
                    CalculatePoints(args.Achievement);
                }
            }
        }

        private void CalculatePoints(Achievement achievement)
        {
            int points = GetDestroyPointsForAchievement(achievement);
            Points += points;
        }

        private int GetDestroyPointsForAchievement(Achievement achievement)
        {
            DestroyPoints destroyPoints = _destroyPoints.FirstOrDefault(x => x.Achievement == achievement);
            return destroyPoints?.Points ?? 0;
        }        
    }
}
