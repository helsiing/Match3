#region
using System;
using System.Collections.Generic;
using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
using Random = UnityEngine.Random;
#endregion

namespace VoodooMatch3.Models
{
    [Serializable]
    public class InitialPiece
    {
        public int X;
        public int Y;
        public PieceTemplate PieceTemplate;
    }

    public partial class LevelTemplate : ScriptableObjectCollectionItem
    {
        [SerializeField] private int width = 7;
        public int Width => width;

        [SerializeField] private int height = 7;
        public int Height => height;
        
        [SerializeField] private int scoreToWin = 100;
        public int ScoreToWin => scoreToWin;
        
        [SerializeField] private int timeToWin = 1000;
        public int TimeToWin => timeToWin;
        
        [SerializeField] public GameObject tilePrefab;
        public GameObject TilePrefab => tilePrefab;

        [SerializeField] public List<PieceTemplate> availableColorPieces;
        public List<PieceTemplate> AvailableColorPieces => availableColorPieces;
        
        [SerializeField] public List<InitialPiece> initialPieces;
        public List<InitialPiece> InitialPieces=> initialPieces;
        
        public PieceTemplate GetRandomColorPieceTemplate()
        {
            int randomIndex = Random.Range(0, AvailableColorPieces.Count);

            return AvailableColorPieces[randomIndex];
        }

        public bool ValidateConfig()
        {
            if (width < 3 || height < 3)
            {
                return false;
            }
            
            if (tilePrefab == null)
            {
                return false;
            }

            if (timeToWin <= 0)
            {
                return false;
            }
            
            if(scoreToWin <= 0)
            {
                return false;
            }
            
            if(availableColorPieces.Count <= 2)
            {
                return false;
            }

            return true;
        }
    }
}