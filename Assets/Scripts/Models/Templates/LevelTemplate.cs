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

        [SerializeField] public int height = 7;
        public int Height => height;

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
    }
}