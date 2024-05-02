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

        if (SelectedChessPiece != null)
        {
            SelectedChessPiece.UpdateAvailableSquares();
        }
    }

    private void _resetSelectedSquare()
    {
        SelectedSquare = null;
        SelectedChessPiece = null;
    }

    private void _onSquareClicked(Square square)
    {
        //First reset old square states
        if (SelectedSquare != null)
        {
            SelectedSquare.SetSquareState(SquareState.Default);
        }

        if (SelectedChessPiece != null)
        {
            foreach (var availableSquare in SelectedChessPiece.AvailableSquares)
            {
                availableSquare.SetSquareState(SquareState.Default);
            }
        }
        /////////////////////////////////

        //Set new selected square
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
        //////////////////////////////

        //Set new square states
        if (SelectedSquare != null)
        {
            SelectedSquare.SetSquareState(SquareState.Selected);
        }

        if (SelectedChessPiece != null)
        {
            foreach (var availableSquare in SelectedChessPiece.AvailableSquares)
            {
                availableSquare.SetSquareState(SquareState.AvailableToMove);
            }
        }
        /////////////////////////////
    }
}
