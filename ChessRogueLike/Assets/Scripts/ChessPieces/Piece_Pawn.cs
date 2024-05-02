using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Pawn : ChessPiece
{
    public override void Init(Square startSquare, Color color)
    {
        base.Init(startSquare, color);

        _pieceType = PieceType.Pawn;
        _materialValue = 1;
    }

    protected override void _checkAvailableSquares()
    {
        base._checkAvailableSquares();

        if (Color == Color.black)
        {
            _checkAvailableSquaresForBlack();
        }
        else if (Color == Color.white)
        {
            _checkAvailableSquaresForWhite();
        }
    }

    private void _checkAvailableSquaresForWhite()
    {
        //TODO: Factor out common code

        //first checks one square ahead
        var squareToCheck = _board.GetAdjacentSquare(_currentSquare, Direction.North);

        if (squareToCheck != null && squareToCheck.CurrentPiece == null)
        {
            AvailableSquares.Add(squareToCheck);

            //if the one ahead was available it also checks 2 squares ahead if its the first move its making
            if (_numberOfMovesMade == 0)
            {
                squareToCheck = _board.GetAdjacentSquare(squareToCheck, Direction.North);

                if (squareToCheck != null && squareToCheck.CurrentPiece == null)
                {
                    AvailableSquares.Add(squareToCheck);
                }
            }
        }

        _checkDiagonal(Direction.NorthEast);
        _checkDiagonal(Direction.NorthWest);

        //TODO: Check for en passant
    }

    private void _checkAvailableSquaresForBlack()
    {
        //first checks one square ahead
        var squareToCheck = _board.GetAdjacentSquare(_currentSquare, Direction.South);

        if (squareToCheck != null && squareToCheck.CurrentPiece == null)
        {
            AvailableSquares.Add(squareToCheck);

            //if the one ahead was available it also checks 2 squares ahead if its the first move its making
            if (_numberOfMovesMade == 0)
            {
                squareToCheck = _board.GetAdjacentSquare(squareToCheck, Direction.South);

                if (squareToCheck != null && squareToCheck.CurrentPiece == null)
                {
                    AvailableSquares.Add(squareToCheck);
                }
            }
        }

        _checkDiagonal(Direction.SouthEast);
        _checkDiagonal(Direction.SouthWest);
    }

    private void _checkDiagonal(Direction direction)
    {
        var squareToCheck = _board.GetAdjacentSquare(_currentSquare, direction);

        if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.Color != Color)
        {
            AvailableSquares.Add(squareToCheck);
        }
    }
}
