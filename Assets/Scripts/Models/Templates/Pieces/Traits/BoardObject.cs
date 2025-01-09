using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    /// <summary>
    /// Trait that indicates the board object that should be instantiated for a piece.
    /// </summary>
    [Serializable]
    public class BoardObject : PieceTrait
    {
        [SerializeField] private GameObject prefab;
        public GameObject Prefab => prefab;
    }
}