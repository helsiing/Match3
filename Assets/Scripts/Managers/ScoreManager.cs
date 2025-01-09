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
        private int scoreToWin;
        
        [SerializeField]
        private int increment;
        
        [SerializeField]
        private ScorePanelUI scorePanelUI;

        public void Init(int scoreToWin)
        {
            this.scoreToWin = scoreToWin;
            scorePanelUI.SetCurrentScoreText(currentScore);
            scorePanelUI.SetScoreToWinText(scoreToWin);
        }

        public void AddScore(int value)
        {
            
            if(currentScore + value > scoreToWin)
            {
                currentScore = scoreToWin;
                Debug.Log("WIN GAME");
            }
            else
            {
                currentScore += value * scoreMultiplier;
                StartCoroutine(CountScoreRoutine());
            }
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
                scorePanelUI.SetCurrentScoreText(counterValue);
                iterations++;
                yield return new WaitForSeconds(0.1f);
            }
            
            counterValue = currentScore;
            scorePanelUI.SetCurrentScoreText(currentScore);
        }
    }
}