namespace Assets.Scripts.PlayerInfo
{
    public class PointsController
    {
        public int Points { get; private set; }

        public void CalculatePoints(Achievement achievement)
        {
            int points = GameData.GetDestroyPointsForAchievement(achievement);
            Points += points;
        }
    }
}
