using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    /// <summary>
    /// Trait that indicates the sprite that should be used for the piece in the UI.
    /// </summary>
    [Serializable]
    public class UiObject : PieceTrait
    {
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;
        
        public override bool ValidateConfig()
        {
            return sprite != null;
        }
    }
}