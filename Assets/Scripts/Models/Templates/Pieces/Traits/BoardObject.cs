using System;
using UnityEngine;

namespace VoodooMatch3.Models.Traits
{
    [Serializable]
    public class BoardObject : PieceTrait
    {
        [SerializeField] private GameObject prefab;
        public GameObject Prefab => prefab;
    }
}