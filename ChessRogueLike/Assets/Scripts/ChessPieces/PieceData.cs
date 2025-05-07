using System;
using UnityEditor.Graphs;
using UnityEngine;

namespace ChessPieces
{
    public class PieceData
    {
        public PieceType PieceType { get; private set; }
        public int MaterialValue { get; private set; }
        
        public Color Color { get; private set; }
        
        //public ChessPiece PiecePrefab { get; private set; }

        public PieceData(PieceType type, Color color)
        {
            PieceType = type;
            Color = color;

            switch (PieceType)
            {
                case PieceType.Pawn:
                    
                    MaterialValue = 1;
                    
                    break;
                case PieceType.Knight:
                    
                    MaterialValue = 3;
                    
                    break;
                case PieceType.Bishop:
                    
                    MaterialValue = 3;
                    
                    break;
                case PieceType.Rook:
                    
                    MaterialValue = 5;
                    
                    break;
                case PieceType.Queen:
                    
                    MaterialValue = 9;
                    
                    break;
                case PieceType.King:
                    
                    MaterialValue = 0;
                    
                    break;
            }
            
            //PiecePrefab = prefabToSpawn;
        }
    }
}
