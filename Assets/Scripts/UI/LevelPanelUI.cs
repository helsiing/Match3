using TMPro;
using UnityEngine;
using VoodooMatch3.Models;

namespace VoodooMatch3.UI
{
    public class LevelPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelName;
        private LevelTemplate levelTemplate;
        
        public void Init(LevelTemplate levelTemplate)
        {
            this.levelTemplate = levelTemplate;
            levelName.text = levelTemplate.name;
        }
    }
}
