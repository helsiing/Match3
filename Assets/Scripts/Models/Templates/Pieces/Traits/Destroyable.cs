using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    /// <summary>
    /// Trait that indicates if a piece can be destroyed on board
    /// </summary>
    [Serializable]
    public class Destroyable : PieceTrait
    {
        [SerializeField]
        private GameObject destroyEffectPrefab;
        public GameObject DestroyEffectPrefab => destroyEffectPrefab;
        
        public override bool ValidateConfig()
        {
            return destroyEffectPrefab != null;
        }
    }
}