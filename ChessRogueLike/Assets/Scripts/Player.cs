using Codice.CM.Client.Differences.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player
{
    public Square SelectedSquare { get; set; }
    public ChessPiece SelectedPiece { get; set; }

    private bool _isPlayerTurn;
    private bool _isMouseDown;
    private bool _squareSelectedThisClick;

    public Player(ChessBoard board)
    {
        SelectedSquare = null;
        SelectedPiece = null;

        board.OnSquareClicked += _onSquareClicked;
        board.OnMouseReleased += _onMouseReleased;
    }

    public void Tick()
    {
        if (_isMouseDown && SelectedPiece != null)
        {
            SelectedPiece.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3.forward * 10);
        }
    }

    private void _setSelectedSquare(Square square)
    {
        SelectedSquare = square;
        SelectedPiece = square.CurrentPiece;

        if (SelectedPiece != null)
        {
            SelectedPiece.UpdateAvailableSquares();
        }
    }

    private void _resetSelectedSquare()
    {
        SelectedSquare = null;
        SelectedPiece = null;
    }

    private void _resetSquareStates()
    {
        if (SelectedSquare != null)
        {
            SelectedSquare.SetSquareState(SquareState.Default);
        }

        if (SelectedPiece != null)
        {
            foreach (var availableSquare in SelectedPiece.AvailableSquares)
            {
                availableSquare.SetSquareState(SquareState.Default);
            }
        }
    }

    private void _onMouseReleased(Square square)
    {
        if (square == null)
        {
            SelectedPiece.transform.position = SelectedSquare.transform.position;
        }

        if (SelectedPiece != null)
        {
            if (SelectedPiece.AvailableSquares.Contains(square))
            {
                _resetSquareStates();

                SelectedPiece.MoveTo(square);
                _resetSelectedSquare();
            }
            else if (square == SelectedSquare)
            {
                SelectedPiece.transform.position = SelectedSquare.transform.position;

                if (!_squareSelectedThisClick)
                {
                    _resetSquareStates();
                    _resetSelectedSquare();
                }
            }
            else
            {
                SelectedPiece.transform.position = SelectedSquare.transform.position;
            }

        }

        _isMouseDown = false;
    }

    private void _onSquareClicked(Square square)
    {
        //First reset old square states
        if (SelectedSquare != null && SelectedSquare != square)
        {
            SelectedSquare.SetSquareState(SquareState.Default);
        }

        if (SelectedPiece != null && SelectedSquare != square)
        {
            foreach (var availableSquare in SelectedPiece.AvailableSquares)
            {
                availableSquare.SetSquareState(SquareState.Default);
            }
        }
        /////////////////////////////////
        
        //Set new selections
        if (square.CurrentPiece != null)
        {
            if (square == SelectedSquare)
            {
                _squareSelectedThisClick = false;
            }
            else
            {
                _squareSelectedThisClick = true;
            }

            _setSelectedSquare(square);
        }
        else if (SelectedPiece != null && SelectedPiece.AvailableSquares.Contains(square))
        {
            SelectedPiece.MoveTo(square);
            _resetSelectedSquare();
        }
        else if (square.CurrentPiece == null)
        {
            _resetSelectedSquare();
        }
        //////////////////////////////

        //set new square states
        if (SelectedSquare != null)
        {
            SelectedSquare.SetSquareState(SquareState.Selected);
        }

        if (SelectedPiece != null)
        {
            foreach (var availableSquare in SelectedPiece.AvailableSquares)
            {
                availableSquare.SetSquareState(SquareState.AvailableToMove);
            }
        }
        //////////////////////////

        _isMouseDown = true;
    }
}
