using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece_Pawn : ChessPiece
{
    public override void Init(Square startSquare)
    {
        base.Init(startSquare);

        _pieceType = PieceType.Pawn;
        _materialValue = 1;
    }

    protected override void _checkAvailableSquares()
    {
        //TODO: Factor out common code

        //first checks one square ahead
        var squareToCheck = _board.GetAdjacentSquare(_currentSquare, Direction.North);

        if (squareToCheck != null && squareToCheck.CurrentPiece == null)
        {
            _availableSquares.Add(squareToCheck);

            //if the one ahead was available it also checks 2 squares ahead if its the first move its making
            if (_numberOfMovesMade == 0)
            {
                squareToCheck = _board.GetAdjacentSquare(squareToCheck, Direction.North);

                if (squareToCheck != null && squareToCheck.CurrentPiece == null)
                {
                    _availableSquares.Add(squareToCheck);
                }
            }
        }

        //Checks diagonals for pieces to take
        squareToCheck = _board.GetAdjacentSquare(_currentSquare, Direction.NorthEast);

        if (squareToCheck != null && squareToCheck.CurrentPiece != null)
        {
            _availableSquares.Add(squareToCheck);
        }

        squareToCheck = _board.GetAdjacentSquare(_currentSquare, Direction.NorthWest);

        if (squareToCheck != null && squareToCheck.CurrentPiece != null)
        {
            _availableSquares.Add(squareToCheck);
        }

        //TODO: Check for en passant
    }
}
