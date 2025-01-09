#region
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
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

        public void SelectPiece()
        {
            if (transform != null)
            {
                transform.DOScale(Vector3.one * 1.2f, 0.2f);
            }
        }

        public IEnumerator PlayLandAnimation(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            
            if (transform != null)
            {
                transform.DOJump(transform.position, .2f, 1, .2f);
            }
        }
    }
}