using UnityEngine;

namespace Assets.Scripts
{
    public class PointsController
    {
        public int Points { get; private set; }

        public void CalculatePoints(Achievement achievement)
        {
            int points = GameData.GetDestroyPointsForAchievement(achievement);
            Debug.Log($"{points}");
            Points += points;
        }
    }
}
