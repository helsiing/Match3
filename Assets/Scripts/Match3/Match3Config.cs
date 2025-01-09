#region
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    [CreateAssetMenu(menuName = "VoodooMatch3/Match3Config", fileName = "Match3Config", order = 0)]
    public class Match3Config : ScriptableObject
    {
        [SerializeField]
        private float swapDuration = 0.5f;
        public float SwapDuration => swapDuration;
        
        [SerializeField]
        private int fillOffsetY = 10;
        public int FillOffsetY => fillOffsetY;
        
        [SerializeField]
        private float fillMoveTime = 0.5f;
        public float FillMoveTime => fillMoveTime;
    }
}