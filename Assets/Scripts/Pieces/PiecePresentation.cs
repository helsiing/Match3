#region
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using VoodooMatch3.Models.Traits;
#endregion

namespace VoodooMatch3
{
    public class PiecePresentation : MonoBehaviour
    {
        private IPiece piece;
        [SerializeField]
        private Transform graphicsTransform;
        
        public void Init(IPiece piece)
        {
            this.piece = piece;
        }
        
        public void PlayDestroyAnimation()
        {
            if (transform != null)
            {
                if (piece.PieceTemplate.TryGetTrait(out Destroyable destroyable))
                {
                    if (destroyable.DestroyEffectPrefab != null)
                    {
                        Debug.Log(destroyable.DestroyEffectPrefab);
                        Instantiate(destroyable.DestroyEffectPrefab, transform.position,
                            Quaternion.identity, transform);
                    }

                }
                graphicsTransform.DOScale(Vector3.zero, 0.2f);
            }
        }

        public void SelectPiece()
        {
            if (transform != null)
            {
                graphicsTransform.DOScale(Vector3.one * 1.2f, 0.2f);
            }
        }

        public IEnumerator PlayLandAnimation(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            
            if (transform != null)
            {
                graphicsTransform.DOJump(transform.position, .2f, 1, .2f);
            }
        }
    }
}