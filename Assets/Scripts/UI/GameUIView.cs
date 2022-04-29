﻿using Assets.Scripts.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coordsText;
        [SerializeField] private TextMeshProUGUI angleText;
        [SerializeField] private TextMeshProUGUI velocityText;
        [SerializeField] private TextMeshProUGUI laserCountText;
        [SerializeField] private TextMeshProUGUI cooldownText;

        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI pointsText;

        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;

        public PlayerStatistics PlayerStatistics { get; set; }

        public void Update()
        {
            if (gameOverPanel.activeSelf)
            {
                return;
            }

            coordsText.text = PlayerStatistics.Coordinates.ToString();
            angleText.text = PlayerStatistics.Angle.ToString();
            velocityText.text = PlayerStatistics.Velocity.ToString("F1");
            laserCountText.text = PlayerStatistics.LaserAmmunitionCount.ToString();
            cooldownText.text = PlayerStatistics.LaserCooldown.ToString("F2") + " сек";
        }

        public void ShowGameOverPanel(int points)
        {
            gameOverPanel.SetActive(true);
            pointsText.text = points.ToString();
        }

        public void AddListenerToContinueButton(UnityAction unityAction)
        {
            continueButton.onClick.AddListener(unityAction);
        }

        public void AddListenerToExitButton(UnityAction unityAction)
        {
            exitButton.onClick.AddListener(unityAction);
        }
    }
}
