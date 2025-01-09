#region
using UnityEngine;
using VoodooMatch3.Models;
#endregion

namespace VoodooMatch3
{
    public interface IPiece
    {
        Vector2Int PositionIndex { get; }
        PieceTemplate PieceTemplate { get; }
        GameObject GameObject { get; }
        void SetParent(Transform parent);
        
        void Init(IBoard board);
        void SetPosition(int x, int y);
        void SetPosition(Vector3 position);
        void Move(int destX, int destY, float moveTime);
        int GetPoints();
    }
    
    [RequireComponent(typeof(PieceSimulation))]
    public class Piece : MonoBehaviour, IPiece
    {
        private Vector2Int positionIndex;
        public Vector2Int PositionIndex => positionIndex;
        public GameObject GameObject => gameObject;

        [SerializeField]
        private PieceTemplate pieceTemplate;
        public PieceTemplate PieceTemplate => pieceTemplate;

        private PieceSimulation pieceSimulation;

        private void Awake()
        {
            pieceSimulation = GetComponent<PieceSimulation>();
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Init(IBoard board)
        {
            pieceSimulation.Init(board, this);
        }

        public void SetPosition(int x, int y)
        {
            positionIndex.x = x;
            positionIndex.y = y;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }

        public void Move(int destX, int destY, float moveTime)
        {
            pieceSimulation.Move(destX, destY, moveTime);
        }

        public int GetPoints()
        {
            return pieceTemplate.Points;
        }
    }
}