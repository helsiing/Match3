using System;

namespace VoodooMatch3.Models.Traits
{
    /// <summary>
    /// Trait that indicates if a piece can move on board
    /// </summary>
    [Serializable]
    public class Movable : PieceTrait
    {
        public override bool ValidateConfig()
        {
            return true;
        }
    }
}