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
    
    [Serializable]
    public class BonusPiece : PieceTrait
    {
        [SerializeField]
        private BonusPieceType bonusPieceType;
        public BonusPieceType BonusPieceType => bonusPieceType;
    }
}