using Codice.CM.Client.Differences.Merge;
using System;
using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player
{
    public Square SelectedSquare { get; private set; }
    public ChessPiece SelectedPiece { get; private set; }

    public int PlayerNum { get; private set; }

    private bool _isPlayerTurn;
    private bool _isMouseDown;
    private bool _squareSelectedThisClick;

    private List<ChessPiece> _pieces;

    public Action OnTurnComplete;

    public Player(ChessBoard board, int playerNum)
    {
        SelectedSquare = null;
        SelectedPiece = null;
        PlayerNum = playerNum;

        board.OnSquareClicked += _onSquareClicked;
        board.OnMouseReleased += _onMouseReleased;

        _isPlayerTurn = false;

        _pieces = new List<ChessPiece>();
    }

    public void SetPlayerTurn()
    {
        _isPlayerTurn = true;
    }

    public void SetPieces(List<ChessPiece> pieces)
    {
        _pieces = pieces;
    }

    public void PromotePiece(ChessPiece oldPiece, ChessPiece newPiece)
    {
        _pieces.Remove(oldPiece);
        _pieces.Add(newPiece);
    }

    public void ClearPieces()
    {
        _pieces.Clear();
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
        if (_pieces.Contains(square.CurrentPiece))
        {
            SelectedSquare = square;
            SelectedPiece = square.CurrentPiece;

            if (SelectedPiece != null)
            {
                SelectedPiece.UpdateAvailableSquares();
            }
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

    private void _movePieceToSquare(Square square)
    {
        _resetSquareStates();

        SelectedPiece.MoveTo(square);
        _resetSelectedSquare();

        OnTurnComplete?.Invoke();
        _isPlayerTurn = false;
    }

    private void _onMouseReleased(Square square)
    {
        if (_isPlayerTurn && GameManager.GameState == GameState.Round)
        {
            if (square == null)
            {
                SelectedPiece.transform.position = SelectedSquare.transform.position;
            }

            if (SelectedPiece != null)
            {
                if (SelectedPiece.AvailableSquares.Contains(square))
                {
                    _movePieceToSquare(square);
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
    }

    private void _onSquareClicked(Square square)
    {
        if (_isPlayerTurn && GameManager.GameState == GameState.Round)
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

                if (SelectedPiece != null && !_pieces.Contains(square.CurrentPiece) && SelectedPiece.AvailableSquares.Contains(square))
                {
                    _movePieceToSquare(square);
                }
                else if(!_pieces.Contains(square.CurrentPiece))
                {
                    _resetSelectedSquare();
                }
                else
                {
                    _setSelectedSquare(square);
                }
            }
            else if (SelectedPiece != null && SelectedPiece.AvailableSquares.Contains(square))
            {
                SelectedPiece.MoveTo(square);
                _resetSelectedSquare();

                OnTurnComplete?.Invoke();
                _isPlayerTurn = false;
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
}
