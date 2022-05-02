using Assets.Scripts.PlayerInfo;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LevelInfo
{
    public class PointsController
    {
        public int Points { get; private set; }

        private List<DestroyPoints> _destroyPoints;

        public PointsController(List<DestroyPoints> destroyPoints)
        {
            _destroyPoints = destroyPoints;
        }

        public void CalculatePoints(Achievement achievement)
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
