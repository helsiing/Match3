using TMPro;
using UnityEngine;
using VoodooMatch3.Models;
using VoodooMatch3.Models.Traits;
using VoodooMatch3.Utils;

namespace VoodooMatch3.UI
{
    public class LevelPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private TMP_Text scoreToWin;
        [SerializeField] private TMP_Text timeToWin;
        [SerializeField] private Transform availablePiecesRoot;
        [SerializeField] private GameObject availablePiecePrefab;
        
        public void SetContent(LevelTemplate levelTemplate)
        {
            levelName.text = $"{levelTemplate.name}\n({levelTemplate.Width} x {levelTemplate.Height})";
            scoreToWin.text = $"Score: {levelTemplate.ScoreToWin.ToString()}";  
            timeToWin.text = $"Time: { levelTemplate.TimeToWin.ToString()}";

            availablePiecesRoot.gameObject.DestroyChildObjects();
            foreach (var pieceTemplate in levelTemplate.availableColorPieces)
            {
                if (pieceTemplate.TryGetTrait(out UiObject uiObject))
                {
                    var availablePieceGameObject = Instantiate(availablePiecePrefab, availablePiecesRoot);
                    LevelPanelAvailablePieceUI availablePieceUI = availablePieceGameObject.GetComponent<LevelPanelAvailablePieceUI>();
                    availablePieceUI.SetContent(uiObject.Sprite);
                }
            }
        }
    }
}
