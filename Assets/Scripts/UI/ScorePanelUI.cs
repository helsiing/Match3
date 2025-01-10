using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using VoodooMatch3.Services;

namespace VoodooMatch3.UI
{
    public class ScorePanelUI : MonoBehaviour
    {
        private IScoreService scoreService;
        
        [SerializeField] private int increment = 10;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text scoreToWinText;

        private Coroutine countCoroutine;

        private int counterValue = 0;

        public void Init()
        {
            ServiceLocator.Global.Get(out scoreService);
            scoreService.OnScoreUpdated += OnScoreUpdated;
            
            SetCurrentScoreText(0);
            SetScoreToWinText(scoreService.GetScoreGoal());
        }
        
        private void OnDestroy()
        {
            scoreService.OnScoreUpdated -= OnScoreUpdated;
        }

        private void OnDisable()
        {
            if (countCoroutine != null)
            {
                StopCoroutine(countCoroutine);
            }
        }

        private void OnScoreUpdated(int previousScore, int newScore)
        {
            if (gameObject.transform.parent.gameObject.activeSelf)
            {
                if (previousScore != newScore)
                {
                    countCoroutine = StartCoroutine(CountScoreRoutine(previousScore, newScore));
                }
                else
                {
                    SetCurrentScoreText(newScore);
                }
            }
        }

        private void SetCurrentScoreText(int scoreValue)
        {
            if (scoreText != null)
            {
                scoreText.text = scoreValue.ToString();
            }
        }
        
        public void SetScoreToWinText(int scoreToWinValue)
        {
            if (scoreText != null)
            {
                scoreToWinText.text = $"(Goal: {scoreToWinValue.ToString()})";
            }
        }
        
        private IEnumerator CountScoreRoutine(int currentScore, int targetScore)
        {
            int iterations = 0;
            counterValue = currentScore;
            while (counterValue < targetScore && iterations < 10000)
            {
                counterValue += increment;
                iterations++;
                SetCurrentScoreText(counterValue);
                yield return new WaitForSeconds(0.01f);
            }
            
            counterValue = currentScore;
            SetCurrentScoreText(targetScore);
        }
    }
}