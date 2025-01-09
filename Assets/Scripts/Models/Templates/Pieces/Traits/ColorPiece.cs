using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    /// <summary>
    /// Trait that indicates that the piece has a color.
    /// </summary>
    [Serializable]
    public class ColorPiece : PieceTrait
    {
        public Color Color;
    }
}   