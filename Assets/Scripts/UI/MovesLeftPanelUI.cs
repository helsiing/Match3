using Managers;
using TMPro;
using UnityEngine;
using VoodooMatch3.Services;

namespace VoodooMatch3.UI
{
    public class MovesLeftPanelUI : MonoBehaviour
    {
        private IScoreService scoreService;
        [SerializeField] private TMP_Text movesLeftText;
        
        private void Start()
        {
            scoreService.OnMovesLeftUpdated += SetCurrentMovesLeftText;
        }

        private void OnDestroy()
        {
            scoreService.OnMovesLeftUpdated -= SetCurrentMovesLeftText;
        }

        private void SetCurrentMovesLeftText(int scoreValue)
        {
            if (movesLeftText != null)
            {
                movesLeftText.text = scoreValue.ToString();
            }
        }

        public void Init()
        {
            ServiceLocator.Global.Get(out scoreService);
            SetCurrentMovesLeftText(scoreService.GetMovesLeft());
        }
    }
}