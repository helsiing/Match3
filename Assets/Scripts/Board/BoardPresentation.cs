#region
using UnityEngine;
using VoodooMatch3.Models;
using VoodooMatch3.Models.Traits;

#endregion

namespace VoodooMatch3
{
    public class BoardPresentation : MonoBehaviour
    {
        private IBoard board;
        private LevelTemplate levelTemplate;
        private Match3Config match3Config;
        
        public void Init(IBoard board, LevelTemplate levelTemplate, Match3Config match3Config)
        {
            this.board = board;
            this.levelTemplate = levelTemplate;
            this.match3Config = match3Config;
        }
        
        public void SetupTiles()
        {
            for (int i = 0; i < levelTemplate.Width; i++)
            {
                for (int j = 0; j < levelTemplate.Height; j++)
                {
                    GameObject tileGameObject = Instantiate(levelTemplate.TilePrefab, new Vector3(i, j, 0f),
                        Quaternion.identity);
                    tileGameObject.name = $"Tile ({i}, {j})";
                    var gridTile = tileGameObject.GetComponent<GridTile>();
                    gridTile.transform.parent = transform;
                    gridTile.Init(board, new Vector2Int(i, j));
                    board.SetGridPieceAt(i, j, gridTile);
                }
            }
        }
        
        public void SetupInitialPieces()
        {
            foreach (InitialPiece initialPiece in levelTemplate.InitialPieces)
            {
                if (initialPiece.PieceTemplate.TryGetTrait(out BoardObject boardObject))
                {
                    GameObject pieceGameObject = Instantiate(boardObject.Prefab,
                        new Vector3(initialPiece.X, initialPiece.Y, 0f), Quaternion.identity);
                    pieceGameObject.name = $"{initialPiece.PieceTemplate.name} ({initialPiece.X}, {initialPiece.Y})";
                    board.SetPieceAt(pieceGameObject, initialPiece.X, initialPiece.Y,
                        match3Config.FillOffsetY, match3Config.FillMoveTime);
                }
            }
        }
    }
}