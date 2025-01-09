using System.Collections;
using UnityEngine;
using VoodooMatch3.Utils;

namespace Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private int currentScore = 0;
        private int counterValue = 0;

        private int scoreMultiplier = 1;
        
        [SerializeField]
        private int increment;
        
        [SerializeField]
        private ScorePanelUI scorePanelUI;

        private void Start()
        {
            scorePanelUI.SetScoreText(currentScore);
        }

        public void AddScore(int value)
        {
            currentScore += value * scoreMultiplier;
            StartCoroutine(CountScoreRoutine());
        }
        
        public void ResetScoreMultiplier()
        {
            scoreMultiplier = 0;
        }
        
        public void IncrementScoreMultiplier()
        {
            scoreMultiplier++;
        }

        IEnumerator CountScoreRoutine()
        {
            int iterations = 0;
            
            while (counterValue < currentScore && iterations < 10000)
            {
                counterValue += increment;
                scorePanelUI.SetScoreText(counterValue);
                iterations++;
                yield return new WaitForSeconds(0.1f);
            }
            
            counterValue = currentScore;
            scorePanelUI.SetScoreText(currentScore);
        }
    }
}