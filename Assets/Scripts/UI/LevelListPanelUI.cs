using UnityEngine;
using UnityEngine.Assertions;
using VoodooMatch3.Models;
using VoodooMatch3.Utils;

namespace VoodooMatch3.UI
{
    public class LevelListPanelUI : MonoBehaviour
    {
        [SerializeField] LevelTemplateCollection levels;
        [SerializeField] GameObject levelPanelPrefab;

        [SerializeField] private Transform gridRoot;
        
        public void LoadLevelList()
        {
            gridRoot.gameObject.DestroyChildObjects();
            
            foreach (LevelTemplate levelTemplate in levels)
            {
                GameObject levelPanelGameObject = Instantiate(levelPanelPrefab, gridRoot);
                LevelPanelUI levelPanelUI = levelPanelGameObject.GetComponent<LevelPanelUI>();
                Assert.IsNotNull(levelPanelUI, "levelPanelUI != null");
                
                levelPanelUI.SetContent(levelTemplate);
                
            }
        }
    }
}