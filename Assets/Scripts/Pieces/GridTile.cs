#region
using UnityEngine;
#endregion

namespace VoodooMatch3
{
    public class GridTile : MonoBehaviour
    {
        private IBoard board;
        private Vector2Int positionIndex;
        public Vector2Int PositionIndex => positionIndex;

        public void Init(IBoard board, Vector2Int positionIndex)
        {
            this.board = board;
            this.positionIndex = positionIndex;
        }

        private void OnMouseDown()
        {
            board.ClickTile(this);
        }

        private void OnMouseEnter()
        {
            board.DragToTile(this);
        }

        private void OnMouseUp()
        {
            board.ReleaseTile();
        }
    }
}