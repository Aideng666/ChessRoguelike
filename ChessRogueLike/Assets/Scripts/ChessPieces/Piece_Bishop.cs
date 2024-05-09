using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Bishop : ChessPiece
{
    public override void Init(Square startSquare, Color color)
    {
        base.Init(startSquare, color);

        _pieceType = PieceType.Bishop;
        MaterialValue = 3;
    }

    private void _checkAvailableSquaresInDirection(Direction direction)
    {
        var squaresToCheck = _board.GetSquaresInDirection(CurrentSquare, direction);

        if (squaresToCheck.Count > 0)
        {
            foreach (var square in squaresToCheck)
            {
                if (square.CurrentPiece != null && square.CurrentPiece.Color == Color)
                {
                    break;
                }
                else if (square.CurrentPiece != null && square.CurrentPiece.Color != Color)
                {
                    AvailableSquares.Add(square);
                    break;
                }

                AvailableSquares.Add(square);
            }
        }
    }

    protected override void _checkAvailableSquares()
    {
        base._checkAvailableSquares();

        _checkAvailableSquaresInDirection(Direction.NorthEast);
        _checkAvailableSquaresInDirection(Direction.NorthWest);
        _checkAvailableSquaresInDirection(Direction.SouthEast);
        _checkAvailableSquaresInDirection(Direction.SouthWest);
    }
}
