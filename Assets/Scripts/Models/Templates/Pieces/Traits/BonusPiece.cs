using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    public enum BonusPieceType
    {
        None,
        Adjacent,
        Horizontal,
        Vertical,
    }
    
    /// <summary>
    /// Trait that indicates that the piece has a trait of bonus piece of a certain type.
    /// </summary>
    [Serializable]
    public class BonusPiece : PieceTrait
    {
        [SerializeField]
        private BonusPieceType bonusPieceType;
        public BonusPieceType BonusPieceType => bonusPieceType;
    }
}