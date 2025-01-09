using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScorePanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        public void SetScoreText(int scoreValue)
        {
            if (scoreText != null)
            {
                scoreText.text = scoreValue.ToString();
            }
        }
    }
}