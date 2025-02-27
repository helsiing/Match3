﻿#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using VoodooMatch3.Models.Traits;
using VoodooMatch3.Services;
#endregion

namespace VoodooMatch3
{
    public class BoardSimulation : MonoBehaviour
    {
        private IBoard board;
        private Match3Config match3Config;

        private GridTile clickedTile;
        private GridTile targetTile;
        private bool playerInputEnabled = true;
        private int width;
        private int height;
        
        private IScoreService scoreService;

        public void Start()
        {
            ServiceLocator.Global.Get(out scoreService);
            scoreService.OnWinGame += OnWinGame;
            scoreService.OnLooseGame += OnLooseGame;
        }

        private void OnDestroy()
        {
            scoreService.OnWinGame -= OnWinGame;
            scoreService.OnLooseGame -= OnLooseGame;
        }

        public void Init(IBoard board, Match3Config match3Config)
        {
            this.board = board;
            this.match3Config = match3Config;
            playerInputEnabled = true;
        }
        
        public void ClickTile(GridTile tile)
        {
            if (clickedTile == null)
            {
                clickedTile = tile;
            }
        }
        
        public void DragToTile(GridTile tile)
        {
            if (targetTile == null && Match3Utils.IsNextTo(tile, clickedTile))
            {
                targetTile = tile;
            }
        }

        public void ReleaseTile()
        {
            if(clickedTile != null && targetTile == null && playerInputEnabled)
            {
                IPiece clickedPiece = board.GetPieceAt(clickedTile.PositionIndex.x, clickedTile.PositionIndex.y);

                var affectedPieces = Match3Utils.GetPiecesAffectedByBonusPiece(board.AllPieces,
                    board.LevelTemplate.Width, board.LevelTemplate.Height, new List<IPiece> {clickedPiece});

                if (affectedPieces.Count > 0)
                {
                    ClearAndRefillBoard(affectedPieces);
                }
                
                board.DecrementMovesLeft();
                clickedTile = null;
            }
            else if (clickedTile != null && targetTile != null)
            {
                SwitchTiles(clickedTile, targetTile);
            }

            clickedTile = null;
            targetTile = null;
        }

        private void SwitchTiles(GridTile clickedTile, GridTile targetTile)
        {
            StartCoroutine(SwitchTilesCoroutine(clickedTile, targetTile));
        }

        private IEnumerator SwitchTilesCoroutine(GridTile clickedTile, GridTile targetTile)
        {
            if (playerInputEnabled)
            {
                IPiece clickedPiece = board.GetPieceAt(clickedTile.PositionIndex.x, clickedTile.PositionIndex.y);
                IPiece targetPiece = board.GetPieceAt(targetTile.PositionIndex.x, targetTile.PositionIndex.y);

                if (targetPiece != null && targetPiece.PieceTemplate.HasTrait<Movable>() 
                    && clickedPiece != null && clickedPiece.PieceTemplate.HasTrait<Movable>())
                {
                    clickedPiece.Move(targetTile.PositionIndex.x, targetTile.PositionIndex.y,
                        match3Config.SwapDuration);
                    targetPiece.Move(clickedTile.PositionIndex.x, clickedTile.PositionIndex.y,
                        match3Config.SwapDuration);

                    yield return new WaitForSeconds(match3Config.SwapDuration);

                    List<IPiece> clickedMatches =
                        Match3Utils.FindMatchesAt(board.AllPieces, board.LevelTemplate.Width, board.LevelTemplate.Height, clickedTile.PositionIndex.x, clickedTile.PositionIndex.y);
                    List<IPiece> targetMatches =
                        Match3Utils.FindMatchesAt(board.AllPieces, board.LevelTemplate.Width, board.LevelTemplate.Height, targetTile.PositionIndex.x, targetTile.PositionIndex.y);

                    board.DecrementMovesLeft();
                    
                    if (clickedMatches.Count == 0 && targetMatches.Count == 0)
                    {
                        clickedPiece.Move(clickedTile.PositionIndex.x, clickedTile.PositionIndex.y,
                            match3Config.SwapDuration);
                        targetPiece.Move(targetTile.PositionIndex.x, targetTile.PositionIndex.y,
                            match3Config.SwapDuration);
                    }
                    else
                    {
                        yield return new WaitForSeconds(match3Config.SwapDuration);
                        ClearAndRefillBoard(clickedMatches.Union(targetMatches).ToList());
                    }
                }
            }
        }

        private void ClearAndRefillBoard(List<IPiece> pieces)
        {
            StartCoroutine(ClearAndRefillBoardCoroutine(pieces));
        }

        private IEnumerator ClearAndRefillBoardCoroutine(List<IPiece> pieces)
        {
            playerInputEnabled = false;

            List<IPiece> matches = pieces;

            do
            {
                yield return StartCoroutine(ClearAndCollapseCoroutine(matches));
                yield return null;

                yield return StartCoroutine(RefillBoardCoroutine());

                matches = board.FindAllMatches();

                yield return new WaitForSeconds(.5f);
            } while (matches.Count != 0);

            playerInputEnabled = true;
        }

        private IEnumerator RefillBoardCoroutine()
        {
            board.FillBoard(match3Config.FillOffsetY, match3Config.FillMoveTime);
            yield return null;
        }

        private IEnumerator ClearAndCollapseCoroutine(List<IPiece> pieces)
        {
            List<IPiece> movingPieces = new List<IPiece>();
            List<IPiece> matches = new List<IPiece>();

            bool isFinished = false;

            scoreService.ResetScoreMultiplier();
            
            while (!isFinished)
            {
                scoreService.IncrementScoreMultiplier();
                
                List<IPiece> affectedPiecesByBonus = Match3Utils.GetPiecesAffectedByBonusPiece(board.AllPieces, board.LevelTemplate.Width, board.LevelTemplate.Height, pieces);
                pieces = pieces.Union(affectedPiecesByBonus).ToList();
                
                yield return StartCoroutine(DestroyPieceAt(pieces));

                movingPieces = board.CollapseColumn(pieces);
                
                while (!Match3Utils.ArePiecesStillCollapsing(movingPieces))
                {
                    yield return null;
                }

                yield return new WaitForSeconds(.25f);

                matches = board.FindMatchesAt(movingPieces);
                if (matches.Count == 0)
                {
                    isFinished = true;
                    break;
                }
                else
                {
                    scoreService.IncrementScoreMultiplier();
                    yield return StartCoroutine(ClearAndCollapseCoroutine(matches));
                }
            }

            yield return null;
        }
        
        private IEnumerator DestroyPieceAt(List<IPiece> pieces)
        {
            List<IPiece> destroyablePieces = pieces.Where(piece => piece != null && piece.PieceTemplate.HasTrait<Destroyable>()).ToList();
            
            foreach (IPiece piece in destroyablePieces.Where(piece => piece != null))
            {
                piece.OnPieceDestroyed();
            }

            yield return new WaitForSeconds(.4f);
            
            foreach (IPiece piece in destroyablePieces.Where(piece => piece != null))
            {
                board.ScorePoints(piece.GetPoints());
                DestroyPieceAt(piece.PositionIndex.x, piece.PositionIndex.y);
            }

            yield return null;
        }
        
        public void DestroyPieceAt(int x, int y)
        {
            if (Match3Utils.IsWithinBounds(board.LevelTemplate.Width, board.LevelTemplate.Height, x, y))
            {
                IPiece pieceToDestroy = board.GetPieceAt(x, y);
                if (pieceToDestroy != null)
                {
                    board.SetEmptyPieceAt(x, y);
                    Destroy(pieceToDestroy.GameObject);
                }    
            }
        }
        
        private void OnLooseGame()
        {
            playerInputEnabled = false;
        }

        private void OnWinGame()
        {
            playerInputEnabled = false;
        }
    }
}