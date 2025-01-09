using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScorePanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text scoreToWinText;
        
        public void SetCurrentScoreText(int scoreValue)
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
    }
}