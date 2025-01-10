using Managers;
using UnityEngine;
using VoodooMatch3.Models;
using VoodooMatch3.Services;

namespace VoodooMatch3.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private LevelListPanelUI levelListPanel;
        [SerializeField] private LevelHudUI levelHudUI;
        [SerializeField] private EndLevelPanelUI endLevelPanelUI;
        
        private IUiService uiService;
        private IScoreService scoreService;

        private void Start()
        {
            ServiceLocator.Global.Get(out uiService);
            ServiceLocator.Global.Get(out scoreService);
            
            uiService.LoadLevel += OnLevelLoad;
            uiService.LoadLevelList += OnLevelListLoad;
            scoreService.OnWinGame += OnWinGame;
            scoreService.OnLooseGame += OnLooseGame;
            
            LoadLevelList();
        }

        private void OnLevelListLoad()
        {
            LoadLevelList();
        }

        private void OnDestroy()
        {
            uiService.LoadLevel -= OnLevelLoad;
            scoreService.OnWinGame -= OnWinGame;
            scoreService.OnLooseGame -= OnLooseGame;
        }

        private void LoadLevelList()
        {
            levelListPanel.gameObject.SetActive(true);
            levelListPanel.Init();
            levelHudUI.gameObject.SetActive(false);
            endLevelPanelUI.gameObject.SetActive(false);
        }

        private void OnLevelLoad(LevelTemplate levelTemplate)
        {
            levelListPanel.gameObject.SetActive(false);
            levelHudUI.gameObject.SetActive(true);
            levelHudUI.Init();
            endLevelPanelUI.gameObject.SetActive(false);
        }
        
        private void OnWinGame()
        {
            levelListPanel.gameObject.SetActive(false);
            levelHudUI.gameObject.SetActive(false);
            endLevelPanelUI.gameObject.SetActive(true);
            endLevelPanelUI.Init(true);
        }
        
        private void OnLooseGame()
        {
            levelListPanel.gameObject.SetActive(false);
            levelHudUI.gameObject.SetActive(false);
            endLevelPanelUI.gameObject.SetActive(true);
            endLevelPanelUI.Init(false);
        }
    }
}