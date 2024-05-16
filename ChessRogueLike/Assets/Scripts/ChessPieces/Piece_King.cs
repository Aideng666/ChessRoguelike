using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using UnityEngine;

public class Piece_King : ChessPiece
{
    public override void Init(Square startSquare, Color color, Player player)
    {
        base.Init(startSquare, color, player);

        _pieceType = PieceType.King;
        MaterialValue = 0;
    }

    private void _checkAvailableAdjacentSquare(Direction direction)
    {
        var squareToCheck = _board.GetAdjacentSquare(CurrentSquare, direction);

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
