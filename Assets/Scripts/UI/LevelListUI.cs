using UnityEngine;
using UnityEngine.Assertions;
using VoodooMatch3.Models;
using VoodooMatch3.Utils;

namespace VoodooMatch3.UI
{
    public class LevelListUI : MonoBehaviour
    {
        [SerializeField] LevelTemplateCollection levels;
        [SerializeField] GameObject levelPanelPrefab;

        [SerializeField] private Transform gridRoot;
        
        private void Start()
        {
            gridRoot.gameObject.DestroyChildObjects();
            
            foreach (var level in levels)
            {
                GameObject levelPanelGameObject = Instantiate(levelPanelPrefab, gridRoot);
                LevelPanelUI levelPanelUI = levelPanelGameObject.GetComponent<LevelPanelUI>();
                Assert.IsNotNull(levelPanelUI, "levelPanelUI != null");
                
                levelPanelUI.Init(level);
                
            }
        }
    }
}