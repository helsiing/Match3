using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoodooMatch3.Services;

namespace VoodooMatch3.UI
{
    public class EndLevelPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button levelListButton;

        private IUiService uiService;
        private void Start()
        {
            ServiceLocator.Global.Get(out uiService);
            levelListButton.onClick.AddListener(OnShowLevelList);
        }

        public void Init(bool winGame)
        {
            if (winGame)
            {
                titleText.text = "You Win =)";
                messageText.text = "Congratulations! You are the best.";
            }
            else
            {
                titleText.text = "You Loose =(";
                messageText.text = "Oh, no! You are still the best.";
            }
        }

        private void OnShowLevelList()
        {
            uiService.LoadLevelList?.Invoke();
        }
    }
}