using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using UnityEngine;

public class Move
{
    public char MovedPiece;
    public char CapturedPiece;
    public Vector2Int StartingSquare;
    public Vector2Int EndingSquare;
    public bool IsPromotion;
    public char PromotionPiece;
}
