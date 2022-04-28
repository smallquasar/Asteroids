using Assets.Scripts.PlayerInfo;
using System;

namespace Assets.Scripts.UI
{
    public class GameUIController
    {
        public Action OnContinueGame;
        public Action OnExitGame;

        private GameUIView _gameUIView;

        public GameUIController(GameUIView gameUIView)
        {
            _gameUIView = gameUIView;
            _gameUIView.AddListenerToContinueButton(ContinueGame);
            _gameUIView.AddListenerToExitButton(ExitGame);
        }

        public void Update(PlayerStatistics playerStatistics)
        {
            _gameUIView.PlayerStatistics = playerStatistics;
        }

        public void PlayerDie(int points)
        {
            _gameUIView.ShowGameOverPanel(points);
        }

        public void ContinueGame()
        {
            OnContinueGame?.Invoke();
        }

        public void ExitGame()
        {
            OnExitGame?.Invoke();
        }
    }
}
