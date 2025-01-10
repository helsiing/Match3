using TMPro;
using UnityEngine;

namespace Managers
{
    public class MovesLeftPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text movesLeftText;
        
        public void SetCurrentMovesLeftText(int scoreValue)
        {
            if (movesLeftText != null)
            {
                movesLeftText.text = scoreValue.ToString();
            }
        }
    }
}