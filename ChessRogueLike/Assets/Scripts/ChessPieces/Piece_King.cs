using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_King : ChessPiece
{
    public override void Init(Square startSquare, Color color)
    {
        base.Init(startSquare, color);

        _pieceType = PieceType.King;
        _materialValue = 0;
    }

    private void _checkAvailableAdjacentSquare(Direction direction)
    {
        var squareToCheck = _board.GetAdjacentSquare(_currentSquare, direction);

        if (squareToCheck != null && squareToCheck.CurrentPiece == null)
        {
            AvailableSquares.Add(squareToCheck);
        }
        else if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.Color != Color)
        {
            AvailableSquares.Add(squareToCheck);
        }
    }

    protected override void _checkAvailableSquares()
    {
        base._checkAvailableSquares();

        _checkAvailableAdjacentSquare(Direction.North);
        _checkAvailableAdjacentSquare(Direction.East);
        _checkAvailableAdjacentSquare(Direction.South);
        _checkAvailableAdjacentSquare(Direction.West);
        _checkAvailableAdjacentSquare(Direction.NorthEast);
        _checkAvailableAdjacentSquare(Direction.NorthWest);
        _checkAvailableAdjacentSquare(Direction.SouthEast);
        _checkAvailableAdjacentSquare(Direction.SouthWest);
    }
}