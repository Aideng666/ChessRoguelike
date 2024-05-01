using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    protected PieceType _pieceType;
    protected int _materialValue;
    protected Square _currentSquare;
    protected ChessBoard _board;

    protected int _numberOfMovesMade = 0;

    public List<Square> AvailableSquares { get; protected set; }

    public virtual void Init(Square startSquare)
    {
        _board = FindObjectOfType<ChessBoard>();
        AvailableSquares = new List<Square>();
        _currentSquare = startSquare;

        MoveTo(startSquare);
    }

    public void MoveTo(Square square)
    {
        _currentSquare.CurrentPiece = null;
        transform.position = square.transform.position;
        _currentSquare = square;
        _currentSquare.CurrentPiece = this;
        _numberOfMovesMade++;
        _checkAvailableSquares();
    }

    protected virtual void _checkAvailableSquares()
    {

    }
}

public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
