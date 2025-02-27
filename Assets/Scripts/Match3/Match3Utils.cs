﻿#region
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoodooMatch3.Models.Traits;

#endregion

namespace VoodooMatch3
{
    public static class Match3Utils
    {
        public static List<IPiece> FindMatches(IPiece[,] allPieces, int width, int height, int startPositionX, int startPositionY, Vector2 searchDirection,
            int minLenght = 3)
        {
            List<IPiece> matches = new List<IPiece>();
            
            List<IPiece> simpleMatches = FindSimpleMatches(allPieces, width, height, startPositionX, startPositionY, searchDirection, minLenght);
            if (simpleMatches != null && simpleMatches.Count > 0)
            {
                matches = matches.Union(simpleMatches).ToList();
            }

            List<IPiece> twoByTwoMatches = new List<IPiece>();
            // Check for 2x2 matches
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    List<IPiece> possibleTwoByTwoMatch = FindTwoByTwoMatches(allPieces, x, y);
                    if(possibleTwoByTwoMatch != null && possibleTwoByTwoMatch.Count > 0)
                    {
                        twoByTwoMatches = twoByTwoMatches.Union(possibleTwoByTwoMatch).ToList();
                    }
                }
            }
            matches = matches.Union(twoByTwoMatches).ToList();
            
            
            return matches;
        }

        private static List<IPiece> FindSimpleMatches(IPiece[,] allPieces, int width, int height, int startPositionX, int startPositionY, Vector2 searchDirection,
            int minLenght = 3)
        {
            IPiece startPiece = null;
            List<IPiece> matches = new List<IPiece>();
            if (IsWithinBounds(width, height, startPositionX, startPositionY))
            {
                startPiece = allPieces[startPositionX, startPositionY];
            }

            if (startPiece != null)
            {
                matches.Add(startPiece);
            }
            else
            {
                return null;
            }

            int maxValue = width > height ? width : height;

            for (int i = 1; i < maxValue; i++)
            {
                int nextPositionX = startPositionX + (int) Mathf.Clamp(searchDirection.x, -1, 1) * i;
                int nextPositionY = startPositionY + (int) Mathf.Clamp(searchDirection.y, -1, 1) * i;

                if (!IsWithinBounds(width, height, nextPositionX,
                        nextPositionY))
                {
                    break;
                }

                IPiece nextPiece = allPieces[nextPositionX, nextPositionY];
                if (nextPiece == null)
                {
                    break;
                }
                else
                {
                    if (nextPiece.PieceTemplate.HasTrait<BonusPiece>())
                    {
                        matches.Add(nextPiece);
                    }
                    else if (startPiece.PieceTemplate.HasTrait<BonusPiece>())
                    {
                        matches.Add(startPiece);
                    }
                    else if (nextPiece.PieceTemplate == startPiece.PieceTemplate && !matches.Contains(nextPiece))
                    {
                        matches.Add(nextPiece);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return matches.Count >= minLenght ? matches : null;
        }
        
        private static List<IPiece> FindTwoByTwoMatches(IPiece[,] allPieces, int x, int y)
        {
            if (allPieces[x, y] == null || allPieces[x + 1, y] == null || 
                allPieces[x, y + 1] == null || allPieces[x + 1, y + 1] == null)
            {
                return null;
            }

            var basePiece = allPieces[x, y];
            if (allPieces[x + 1, y].PieceTemplate == basePiece.PieceTemplate &&
                allPieces[x, y + 1].PieceTemplate == basePiece.PieceTemplate &&
                allPieces[x + 1, y + 1].PieceTemplate == basePiece.PieceTemplate)
            {
                return new List<IPiece>
                {
                    basePiece,
                    allPieces[x + 1, y],
                    allPieces[x, y + 1],
                    allPieces[x + 1, y + 1]
                };
            }

            return null;
        }

        private static List<IPiece> FindVerticalMatches(IPiece[,] allPieces, int width, int height, int startPositionX, int startPositionY, int minLenght = 3)
        {
            List<IPiece> upwardMatches = FindMatches(allPieces, width, height, startPositionX, startPositionY, Vector2.up, 2);
            List<IPiece> downwardMatches = FindMatches(allPieces, width, height, startPositionX, startPositionY, Vector2.down, 2);

            if (upwardMatches == null)
            {
                upwardMatches = new List<IPiece>();
            }

            if (downwardMatches == null)
            {
                downwardMatches = new List<IPiece>();
            }

            var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

            return combinedMatches.Count >= minLenght ? combinedMatches : null;
        }
        
        private static List<IPiece> FindHorizontalMatches(IPiece[,] allPieces, int width, int height, int startPositionX, int startPositionY, int minLenght = 3)
        {
            List<IPiece> rightMatches = FindMatches(allPieces, width, height, startPositionX, startPositionY, Vector2.right, 2);
            List<IPiece> leftMatches = FindMatches(allPieces, width, height, startPositionX, startPositionY, Vector2.left, 2);

            if (rightMatches == null)
            {
                rightMatches = new List<IPiece>();
            }

            if (leftMatches == null)
            {
                leftMatches = new List<IPiece>();
            }

            var combinedMatches = rightMatches.Union(leftMatches).ToList();
            return combinedMatches.Count >= minLenght ? combinedMatches : null;
        }
        
        public static List<IPiece> FindMatchesAt(IPiece[,] allPieces, int width, int height, int x, int y, int minLenght = 3)
        {
            List<IPiece> matchesPieces = new List<IPiece>();
            List<IPiece> horizontalMatches = FindHorizontalMatches(allPieces, width, height, x, y, minLenght);
            List<IPiece> verticalMatches = FindVerticalMatches(allPieces, width, height, x, y, minLenght);

            if (horizontalMatches == null)
            {
                horizontalMatches = new List<IPiece>();
            }

            if (verticalMatches == null)
            {
                verticalMatches = new List<IPiece>();
            }

            matchesPieces = matchesPieces.Union(horizontalMatches).ToList();
            matchesPieces = matchesPieces.Union(verticalMatches).ToList();
            
            IPiece possibleBonusPiece = allPieces[x, y];
            if (possibleBonusPiece != null && possibleBonusPiece.PieceTemplate.TryGetTrait(out BonusPiece bonusPiece))
            {
                matchesPieces = matchesPieces.Union(GetPiecesAffectedByBonusPiece(allPieces, width, height, new List<IPiece> {possibleBonusPiece})).ToList();
            }

            return matchesPieces;
        }
        
        public static bool IsWithinBounds(int width, int height, int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public static bool IsNextTo(GridTile start, GridTile end)
        {
            if (start == null)
            {
                return false;
            }

            if (end == null)
            {
                return false;
            }

            if (Mathf.Abs(start.PositionIndex.x - end.PositionIndex.x) == 1)
            {
                if (start.PositionIndex.y == end.PositionIndex.y)
                {
                    return true;
                }
            }

            if (Mathf.Abs(start.PositionIndex.y - end.PositionIndex.y) == 1)
            {
                if (start.PositionIndex.x == end.PositionIndex.x)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<int> GetColumnsAffectedByMatches(List<IPiece> pieces)
        {
            List<int> affectedColumns = new List<int>();

            foreach (IPiece piece in pieces)
            {
                if (piece != null && !affectedColumns.Contains(piece.PositionIndex.x))
                {
                    affectedColumns.Add(piece.PositionIndex.x);
                }
            }

            return affectedColumns;
        }

        public static bool ArePiecesStillCollapsing(List<IPiece> pieces)
        {
            foreach (IPiece piece in pieces)
            {
                if (piece != null && piece.IsPieceFalling())
                {
                    return false;
                }
            }

            return true;
        }
        
        public static List<IPiece> GetRowPieces(IPiece[,] allPieces, int width, int row)
        {
            List<IPiece> rowPieces = new List<IPiece>();

            for (int x = 0; x < width; x++)
            {
                rowPieces.Add(allPieces[x, row]);
            }

            return rowPieces;
        }

        private static List<IPiece> GetColumnPieces(IPiece[,] allPieces, int height, int column)
        {
            List<IPiece> columnPieces = new List<IPiece>();

            for (int y = 0; y < height; y++)
            {
                columnPieces.Add(allPieces[column, y]);
            }

            return columnPieces;
        }

        private static List<IPiece> GetAdjacentPieces(IPiece[,] allPieces, int width, int height, int x, int y, int offset = 1)
        {
            List<IPiece> adjacentPieces = new List<IPiece>();
            for(int i = x - offset; i <= x + offset; i++)
            {
                for(int j = y - offset; j <= y + offset; j++)
                {
                    if (IsWithinBounds(width, height, i, j))
                    {
                        adjacentPieces.Add(allPieces[i, j]);
                    }
                }
            }

            return adjacentPieces;
        }
        
        public static List<IPiece> GetPiecesAffectedByBonusPiece(IPiece[,] allPieces, int width, int height, List<IPiece> pieces)
        {
            List<IPiece> affectedPieces = new List<IPiece>();

            foreach (IPiece piece in pieces)
            {
                if (piece != null)
                {
                    List<IPiece> affectedPiecesByBonus = new List<IPiece>();

                    if (piece.PieceTemplate.TryGetTrait(out BonusPiece bonusPiece))
                    {
                        switch (bonusPiece.BonusPieceType)
                        {
                            case BonusPieceType.Adjacent:
                                affectedPiecesByBonus = GetAdjacentPieces(allPieces, width,
                                    height, piece.PositionIndex.x, piece.PositionIndex.y);
                                break;
                            case BonusPieceType.Horizontal:
                                affectedPiecesByBonus = GetRowPieces(allPieces, width, piece.PositionIndex.y);
                                break;
                            case BonusPieceType.Vertical:
                                affectedPiecesByBonus = GetColumnPieces(allPieces, height, piece.PositionIndex.x);
                                break;
                        }
                        affectedPieces = affectedPieces.Union(affectedPiecesByBonus).ToList();
                    }
                }
            }
            return affectedPieces;
        }
    }
}