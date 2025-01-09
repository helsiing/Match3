#region

using System.Collections.Generic;
using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
#endregion

namespace VoodooMatch3.Models
{
    public class PieceTemplate : ScriptableObjectCollectionItem
    {
        [SerializeField] private int points;
        public int Points => points;
        
        [SerializeReference, SubclassSelector]
        private List<PieceTrait> traits = new ();
        public List<PieceTrait> Traits => traits;
        
        public bool TryGetTrait<T>(out T trait) where T : PieceTrait
        {
            
            int index = traits.FindIndex(x => x.GetType() == typeof(T));
            if (index < 0)
            {
                trait = null;
                return false;
            }

            trait = traits[index] as T;
            return true;
        }
        
        public bool HasTrait<T>() where T : PieceTrait
        {
            return TryGetTrait(out T _);
        }

        public bool ValidateConfig()
        {
            foreach (PieceTrait trait in traits)
            {
                Debug.Log(trait);
                if (!trait.ValidateConfig())
                {
                    return false;
                }
            }

            return true;
        }
    }
}