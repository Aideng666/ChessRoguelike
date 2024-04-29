using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    protected PieceType _pieceType;
    protected int _materialValue;
    protected Square _currentSquare;
    protected List<Square> _availableSquares = new List<Square>();
    protected ChessBoard _board;

    protected int _numberOfMovesMade = 0;

    public bool IsSelected { get; set; }

    public virtual void Init(Square startSquare)
    {
        _board = FindObjectOfType<ChessBoard>();
        _currentSquare = startSquare;
        _checkAvailableSquares();
    }

    protected void MoveTo(Square square)
    {
        transform.position = square.transform.position;
        _currentSquare = square;
        _numberOfMovesMade++;
    }

    protected virtual void _checkAvailableSquares()
    {

    }

    private void OnMouseDown()
    {
        IsSelected = !IsSelected;

        print($"IsSelected: {IsSelected}");
        print($"Available Squares:");

        foreach (Square square in _availableSquares)
        {
            print($"{square.File}{square.Rank}");
        }
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
