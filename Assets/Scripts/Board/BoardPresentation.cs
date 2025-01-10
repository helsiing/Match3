#region
using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using VoodooMatch3.Models;
using VoodooMatch3.Models.Traits;
using VoodooMatch3.Services;
using VoodooMatch3.Utils;

#endregion

namespace VoodooMatch3
{
    public class BoardPresentation : MonoBehaviour
    {
        [SerializeField] private Transform boardRoot;
        [SerializeField] private Transform tileGridRoot;
        [SerializeField] private Transform piecesGridRoot;
        
        private IBoard board;
        private LevelTemplate levelTemplate;
        private Match3Config match3Config;
        private IScoreService scoreService;
        private IUiService uiService;

        private void Start()
        {
            ServiceLocator.Global.Get(out scoreService);
            ServiceLocator.Global.Get(out uiService);
            scoreService.OnWinGame += HideBoard;
            scoreService.OnLooseGame += HideBoard;
            uiService.LoadLevelList += HideBoard;
        }
        
        private void OnDestroy()
        {
            scoreService.OnWinGame -= HideBoard;
            scoreService.OnLooseGame -= HideBoard;            
            uiService.LoadLevelList -= HideBoard;
        }

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
                        Quaternion.identity, tileGridRoot);
                    tileGameObject.name = $"Tile ({i}, {j})";
                    var gridTile = tileGameObject.GetComponent<GridTile>();
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
                        new Vector3(initialPiece.X, initialPiece.Y, 0f), Quaternion.identity, piecesGridRoot);
                    pieceGameObject.name = $"{initialPiece.PieceTemplate.name} ({initialPiece.X}, {initialPiece.Y})";
                    board.SetPieceAt(pieceGameObject, initialPiece.X, initialPiece.Y,
                        match3Config.FillOffsetY, match3Config.FillMoveTime);
                }
            }
        }

        public GameObject SetPieceAt(GameObject prefab, int x, int y)
        {
            return Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity, piecesGridRoot);
        }

        public void ShowBoard()
        {
            boardRoot.localScale = Vector3.one;
        }
        
        public void HideBoard()
        {
            boardRoot.localScale = Vector3.zero;
        }

        public void ClearBoard()
        {
            tileGridRoot.gameObject.DestroyChildObjects();
            piecesGridRoot.gameObject.DestroyChildObjects();
        }
    }
}