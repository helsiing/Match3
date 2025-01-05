#region
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoodooMatch3.Models;
using VoodooMatch3.Models.Traits;

#endregion

namespace VoodooMatch3
{
    public interface IBoard
    {
        public LevelTemplate LevelTemplate { get; }
        public IPiece[,] AllPieces { get; }
        public void ClickTile(GridTile tile);
        public void DragToTile(GridTile tile);
        public void ReleaseTile();
        public IPiece GetPieceAt(int x, int y);
        public void SetGridPieceAt(int x, int y, GridTile tile);
        public void PlacePiece(IPiece piece, int x, int y);
        public List<IPiece> FindMatchesAt(List<IPiece> pieces, int minLenght = 3);
        public List<IPiece> FindAllMatches();
        public void FillBoard(int falseYOffset = 0, float moveTime = 0.1f);
        public void DestroyPieceAt(List<IPiece> pieces);
        public List<IPiece> CollapseColumn(List<IPiece> piece);

        public Piece SetPieceAt(GameObject randomPiece,
            PieceTemplate randomPieceTemplate, int x, int y, int falseYOffset, float moveTime);
    }
    
    [RequireComponent(typeof(BoardPresentation))]
    [RequireComponent(typeof(BoardSimulation))]
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField] private BoardCamera boardCamera;
        [SerializeField] private LevelTemplate levelTemplate;
        public LevelTemplate LevelTemplate => levelTemplate;

        [SerializeField] private Match3Config match3Config;
        
        private GridTile[,] allTiles;
        private IPiece[,] allPieces;
        public IPiece[,] AllPieces => allPieces;
        
        private BoardPresentation boardPresentation;
        private BoardSimulation boardSimulation;

        private void Awake()
        {
            boardPresentation = GetComponent<BoardPresentation>();
            boardSimulation = GetComponent<BoardSimulation>();
        }

        public void Start()
        {
            allTiles = new GridTile[levelTemplate.Width, levelTemplate.Height];
            allPieces = new IPiece[levelTemplate.Width, levelTemplate.Height];

            boardSimulation.Init(this, match3Config);
            boardPresentation.Init(this, levelTemplate, match3Config);
            
            boardCamera.SetupCamera(levelTemplate.Width, levelTemplate.Height);

            boardPresentation.SetupTiles();
            boardPresentation.SetupInitialPieces();
            
            FillBoard(match3Config.FillOffsetY, match3Config.FillMoveTime);
        }
        
        public IPiece GetPieceAt(int x, int y)
        {
            return allPieces[x, y];
        }
        
        public void SetGridPieceAt(int x, int y, GridTile tile)
        {
            allTiles[x, y] = tile;
        }
        
        public void PlacePiece(IPiece piece, int x, int y)
        {
            if (piece != null)
            {
                piece.SetPosition(new Vector3(x, y, 0));
                if (Match3Utils.IsWithinBounds(levelTemplate.Width, levelTemplate.height, x, y))
                {
                    allPieces[x, y] = piece;
                }

                piece.SetPosition(x, y);
            }
        }

        public void FillBoard(int falseYOffset = 0, float moveTime = 0.1f)
        {
            int maxIterations = 100;
            int iterations = 0;

            for (int i = 0; i < levelTemplate.Width; i++)
            {
                for (int j = 0; j < levelTemplate.Height; j++)
                {
                    if (allPieces[i, j] == null)
                    {
                        Piece piece = FillRandomAt(i, j, falseYOffset, moveTime);
                        iterations = 0;

                        while (HasMatchOnFill(i, j) && iterations < maxIterations)
                        {
                            DestroyPieceAt(i, j);
                            piece = FillRandomAt(i, j, falseYOffset, moveTime);
                            iterations++;
                        }
                    }
                }
            }
        }

        private Piece FillRandomAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
        {
            PieceTemplate randomPieceTemplate = levelTemplate.GetRandomColorPieceTemplate();

            if (randomPieceTemplate.TryGetTrait(out BoardObject boardObject))
            {
                GameObject randomPiece =
                    Instantiate(boardObject.Prefab, new Vector3(x, y, 0f), Quaternion.identity);

                return SetPieceAt(randomPiece, randomPieceTemplate, x, y, falseYOffset, moveTime);
            }

            return null;
        }

        public Piece SetPieceAt(GameObject pieceGameObject,
            PieceTemplate pieceTemplate, int x, int y, int falseYOffset, float moveTime)
        {
            if (pieceGameObject != null)
            {
                Piece piece = pieceGameObject.GetComponent<Piece>();
                if (piece != null)
                {
                    piece.Init(this);
                    PlacePiece(piece, x, y);

                    if (falseYOffset != 0)
                    {
                        piece.transform.position = new Vector3(x, y + falseYOffset, 0f);
                        piece.Move(x, y, moveTime);
                    }

                    piece.name = $"{pieceTemplate.name} ({x}, {y})";
                    piece.transform.parent = transform;
                    return piece;
                }
            }
            return null;
        }

        public void ClickTile(GridTile tile)
        {
            boardSimulation.ClickTile(tile);
        }

        private bool HasMatchOnFill(int x, int y, int minLeght = 3)
        {
            List<IPiece> leftMatches = FindMatches(x, y, Vector2.left, minLeght);
            List<IPiece> downwardMatches = FindMatches(x, y, Vector2.down, minLeght);

            if (leftMatches == null)
            {
                leftMatches = new List<IPiece>();
            }

            if (downwardMatches == null)
            {
                downwardMatches = new List<IPiece>();
            }

            return leftMatches.Count > 0 || downwardMatches.Count > 0;
        }

        public void DragToTile(GridTile tile)
        {
            boardSimulation.DragToTile(tile);
        }

        public void ReleaseTile()
        {
            boardSimulation.ReleaseTile();
        }

        private List<IPiece> FindMatches(int startPositionX, int startPositionY, Vector2 searchDirection,
            int minLenght = 3)
        {
            return Match3Utils.FindMatches(allPieces, levelTemplate.Width, levelTemplate.height, startPositionX,
                startPositionY, searchDirection, minLenght);
        }
        
        public List<IPiece> FindMatchesAt(List<IPiece> pieces, int minLenght = 3)
        {
            List<IPiece> matches = new List<IPiece>();
            return pieces.Select(piece => Match3Utils.FindMatchesAt(allPieces, levelTemplate.Width, levelTemplate.Height, piece.PositionIndex.x, piece.PositionIndex.y, minLenght))
                .Aggregate(matches, (current, pieceMatches) => current.Union(pieceMatches).ToList());
        }

        public List<IPiece> FindAllMatches()
        {
            List<IPiece> combinedMatches = new List<IPiece>();

            for (int i = 0; i < levelTemplate.Width; i++)
            {
                for (int j = 0; j < levelTemplate.Height; j++)
                {
                    List<IPiece> matches = Match3Utils.FindMatchesAt(allPieces, levelTemplate.Width, levelTemplate.Height, i, j);
                    combinedMatches = combinedMatches.Union(matches).ToList();
                }
            }

            return combinedMatches;
        }

        private void DestroyPieceAt(int x, int y)
        {
            IPiece pieceToDestroy = allPieces[x, y];
            if (pieceToDestroy != null)
            {
                allPieces[x, y] = null;
                Destroy(pieceToDestroy.GameObject);
            }
        }

        public void DestroyPieceAt(List<IPiece> pieces)
        {
            foreach (IPiece piece in pieces)
            {
                if (piece != null)
                {
                    DestroyPieceAt(piece.PositionIndex.x, piece.PositionIndex.y);
                }
            }
        }

        public void ClearBoard()
        {
            for (int i = 0; i < levelTemplate.Width; i++)
            {
                for (int j = 0; j < levelTemplate.Height; j++)
                {
                    DestroyPieceAt(i, j);
                }
            }
        }

        private List<IPiece> CollapseColumn(int columnIndex, float collapseDuration = 0.1f)
        {
            List<IPiece> movingPieces = new List<IPiece>();
            for (int i = 0; i < levelTemplate.Height - 1; i++)
            {
                if (allPieces[columnIndex, i] == null)
                {
                    for (int j = i + 1; j < levelTemplate.Height; j++)
                    {
                        if (allPieces[columnIndex, j] != null)
                        {
                            allPieces[columnIndex, j].Move(columnIndex, i, collapseDuration * (j - i));
                            allPieces[columnIndex, i] = allPieces[columnIndex, j];
                            allPieces[columnIndex, i].SetPosition(columnIndex, i);

                            if (!movingPieces.Contains(allPieces[columnIndex, i]))
                            {
                                movingPieces.Add(allPieces[columnIndex, i]);
                            }

                            allPieces[columnIndex, j] = null;
                            break;
                        }
                    }
                }
            }

            return movingPieces;
        }

        public List<IPiece> CollapseColumn(List<IPiece> piece)
        {
            List<IPiece> movingPieces = new List<IPiece>();
            List<int> affectedColumns = Match3Utils.GetColumnsAffectedByMatches(piece);

            foreach (int column in affectedColumns)
            {
                movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
            }

            return movingPieces;
        }
    }
}