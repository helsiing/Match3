#region

using UnityEngine;
#endregion

namespace VoodooMatch3.Models
{
    [CreateAssetMenu(menuName = "VoodooMatch3/PieceTemplate", fileName = "PieceTemplate", order = 0)]
    public class PieceTemplate : ScriptableObject
    {
        [SerializeField] private int points;
        public int Points => points;
        
        
        [SerializeReference, SubclassSelector]
        public PieceTrait[] Traits;
        
        public bool TryGetTrait<T>(out T trait) where T : PieceTrait
        {
            int index = -1;
            for(int i = 0; i < Traits.Length; i++)
            {
                if (Traits[i].GetType() == typeof(T))
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                trait = null;
                return false;
            }

            trait = Traits[index] as T;
            return true;
        }
        
        public bool HasTrait<T>() where T : PieceTrait
        {
            return TryGetTrait(out T _);
        }

        public bool ValidateConfig()
        {
            foreach (PieceTrait trait in Traits)
            {
                if (!trait.ValidateConfig())
                {
                    return false;
                }
            }

            return true;
        }
    }
}