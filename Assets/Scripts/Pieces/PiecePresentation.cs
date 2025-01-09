#region
using DG.Tweening;
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    public class PiecePresentation : MonoBehaviour
    {
        public void PlayDestroyAnimation()
        {
            if (transform != null)
            {
                transform.DOScale(Vector3.zero, 0.2f);
            }
        }
    }
}