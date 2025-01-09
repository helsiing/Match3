using UnityEngine;
using UnityEngine.UI;
using VoodooMatch3.Models;
using VoodooMatch3.Models.Traits;

namespace VoodooMatch3.UI
{
    public class LevelPanelAvailablePieceUI : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetContent(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}