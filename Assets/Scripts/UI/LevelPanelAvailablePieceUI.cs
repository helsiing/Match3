using UnityEngine;
using UnityEngine.UI;

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