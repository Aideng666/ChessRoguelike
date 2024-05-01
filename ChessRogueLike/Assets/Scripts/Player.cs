using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Square SelectedSquare { get; set; }
    public ChessPiece SelectedChessPiece { get; set; }

    private bool _isPlayerTurn;

    public Player(ChessBoard board)
    {
        SelectedSquare = null;
        SelectedChessPiece = null;

        board.OnSquareClicked += _onSquareClicked;
    }

    private void _setSelectedSquare(Square square)
    {
        SelectedSquare = square;
        SelectedChessPiece = square.CurrentPiece;
    }

    private void _resetSelectedSquare()
    {
        SelectedSquare = null;
        SelectedChessPiece = null;
    }

    private void _onSquareClicked(Square square)
    {
        if (SelectedSquare == square)
        {
            _resetSelectedSquare();
        }
        else if (SelectedChessPiece != null && SelectedChessPiece.AvailableSquares.Contains(square))
        {
            SelectedChessPiece.MoveTo(square);
            _resetSelectedSquare();
        }
        else
        {
            _setSelectedSquare(square);
        }

    }
}
