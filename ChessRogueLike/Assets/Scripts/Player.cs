using Codice.CM.Client.Differences.Merge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChessPieces;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

public class Player : IPlayer
{
    public Square SelectedSquare { get; private set; }
    public ChessPiece SelectedPiece { get; private set; }

    public int PlayerNum { get; private set; }

    private bool _isPlayerTurn;
    private bool _isMouseDown;
    private bool _squareSelectedThisClick;

    private MinimaxAI _ai;
    private ChessBoard _board;
    private bool _isAi;
    private bool _isWhite;
    private int _aiDepth;
    
    private List<ChessPiece> _takenOpponentPieces;
    public List<ChessPiece> ActivePieces { get; private set; }

    public Action OnTurnComplete;

    public Player(ChessBoard board, int playerNum)
    {
        SelectedSquare = null;
        SelectedPiece = null;
        PlayerNum = playerNum;
        _board = board;

        board.OnSquareClicked += _onSquareClicked;
        board.OnMouseReleased += _onMouseReleased;

        _isPlayerTurn = false;

        ActivePieces = new List<ChessPiece>();
        _takenOpponentPieces = new List<ChessPiece>();
    }

    public void SetPlayerTurn()
    {
        _isPlayerTurn = true;
    }

    public void SetPieces(List<ChessPiece> pieces)
    {
        ActivePieces = pieces;
    }

    public void RemovePiece(ChessPiece piece)
    {
        if (ActivePieces.Contains(piece))
        {
            ActivePieces.Remove(piece);
            GameObject.Destroy(piece.gameObject);
        }
    }

    public void AddTakenPiece(ChessPiece piece)
    {
        _takenOpponentPieces.Add(piece);
    }

    public void PromotePiece(ChessPiece oldPiece, ChessPiece newPiece)
    {
        ActivePieces.Remove(oldPiece);
        ActivePieces.Add(newPiece);
    }

    public void ClearPieces()
    {
        ActivePieces.Clear();
    }

    public void Tick()
    {
        if (_isAi && _isPlayerTurn)
        {
            var moveToMake = _ai.GetBestMove(_aiDepth, _isWhite, _board);
            _board.Board[moveToMake.StartingSquare.x, moveToMake.StartingSquare.y].CurrentPiece.MoveTo(_board.Board[moveToMake.EndingSquare.x, moveToMake.EndingSquare.y]);
            
            OnTurnComplete?.Invoke();
            _isPlayerTurn = false;
        }
        else if (_isMouseDown && SelectedPiece != null)
        {
            SelectedPiece.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3.forward * 10);
        }
    }

    public void SetAIPlayer(MinimaxAI ai, int aiDepth, bool isWhite)
    {
        _isAi = true;
        _ai = ai;
        _aiDepth = aiDepth;
        _isWhite = isWhite;
    }

    private void _setSelectedSquare(Square square)
    {
        if (ActivePieces.Contains(square.CurrentPiece))
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
        if (_isPlayerTurn && GameManager.GameState == GameState.Round && PlayerNum == 1)
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
        if (_isPlayerTurn && GameManager.GameState == GameState.Round && PlayerNum == 1)
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

                if (SelectedPiece != null && !ActivePieces.Contains(square.CurrentPiece) && SelectedPiece.AvailableSquares.Contains(square))
                {
                    _movePieceToSquare(square);
                }
                else if(!ActivePieces.Contains(square.CurrentPiece))
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
