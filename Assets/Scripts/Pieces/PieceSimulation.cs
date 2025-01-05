#region
using System.Collections;
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    public class PieceSimulation : MonoBehaviour
    {
        private IBoard board;
        private Piece piece;
        private bool isMoving = false;
        
        public void Init(IBoard board, Piece piece)
        {
            this.board = board;
            this.piece = piece;
        }
        
        public void Move(int destX, int destY, float timeToMove)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0f), timeToMove));
            }
        }

        private IEnumerator MoveRoutine(Vector3 dest, float timeToMove)
        {
            Vector3 startPos = transform.position;
            bool hasReachedDestination = false;
            float elapsedTime = 0f;

            isMoving = true;
            while (!hasReachedDestination)
            {
                if (Vector3.Distance(transform.position, dest) < 0.01f)
                {
                    hasReachedDestination = true;
                    board.PlacePiece(piece, (int) dest.x, (int) dest.y);
                    break;
                }

                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
                t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(startPos, dest, t);

                yield return null;
            }

            isMoving = false;
        }
        
    }
}