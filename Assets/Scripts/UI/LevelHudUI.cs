using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using VoodooMatch3.Services;

namespace VoodooMatch3.UI
{
    public class LevelHudUI : MonoBehaviour
    {
        [SerializeField] private ScorePanelUI scorePanelUI;
        [SerializeField] private MovesLeftPanelUI movesLeftPanelUI;
        [SerializeField] private Button levelListButton;

        private IUiService uiService;
        
        public void Start()
        {
            ServiceLocator.Global.Get(out uiService);
            levelListButton.onClick.AddListener(OnShowLevelList);
        }
        
        private void OnShowLevelList()
        {
            uiService.LoadLevelList?.Invoke();
        }
    }
}