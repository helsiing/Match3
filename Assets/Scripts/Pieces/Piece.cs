#region
using System;
using UnityEngine;
using UnityEngine.Assertions;
using VoodooMatch3.Models;
#endregion

namespace VoodooMatch3
{
    public interface IPiece
    {
        Vector2Int PositionIndex { get; }
        PieceTemplate PieceTemplate { get; }
        GameObject GameObject { get; }
        public bool IsPieceFalling();
        void SetParent(Transform parent);
        void Init(IBoard board);
        void SetPosition(int x, int y);
        void SetPosition(Vector3 position);
        void Move(int destX, int destY, float moveTime, bool isMoveCollapsing = false);
        int GetPoints();
        void OnPieceDestroyed();
        void OnPieceSelected();
    }
    
    [RequireComponent(typeof(PiecePresentation))]
    [RequireComponent(typeof(PieceSimulation))]
    public class Piece : MonoBehaviour, IPiece
    {
        private Vector2Int positionIndex;
        public Vector2Int PositionIndex => positionIndex;
        public GameObject GameObject => gameObject;

        [SerializeField]
        private PieceTemplate pieceTemplate;
        public PieceTemplate PieceTemplate => pieceTemplate;

        private PiecePresentation piecePresentation;
        private PieceSimulation pieceSimulation;

        private void Awake()
        {
            pieceSimulation = GetComponent<PieceSimulation>();
            Assert.IsNotNull(pieceSimulation, "pieceSimulation != null");
            
            piecePresentation = GetComponent<PiecePresentation>();
            Assert.IsNotNull(piecePresentation, "piecePresentation != null");
        }

        public bool IsPieceFalling()
        {
            return !(transform.position.y - (float) PositionIndex.y > 0.001f);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Init(IBoard board)
        {
            pieceSimulation.Init(board, this);
            piecePresentation.Init(this);
        }

        public void SetPosition(int x, int y)
        {
            gameObject.name = $"{pieceTemplate.name} ({x}, {y})";
            positionIndex.x = x;
            positionIndex.y = y;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }

        public void Move(int destX, int destY, float moveTime, bool isMoveCollapsing = false)
        {
            pieceSimulation.Move(destX, destY, moveTime);
            if (isMoveCollapsing)
            {
                StartCoroutine(piecePresentation.PlayLandAnimation(moveTime));
            }
        }

        public int GetPoints()
        {
            return pieceTemplate.Points;
        }

        public void OnPieceDestroyed()
        {
            if (piecePresentation != null)
            {
                piecePresentation.PlayDestroyAnimation();
            }
        }

        public void OnPieceSelected()
        {
            if (piecePresentation != null)
            {
                piecePresentation.SelectPiece();
            }
        }
    }
}