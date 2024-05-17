using System;
using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ChessBoard _boardPrefab;

    private ChessBoard _board;
    private Player _player1;
    private Player _player2;
    private int _roundNumber;
    private List<PieceData> _player1Pieces;
    private int _currentPiecePlacementIndex;
    private int _startingPlayer;
    private List<ChessPiece> _spawnedPlayerPieces;

    public int Turn { get; private set; }

    public static GameState GameState { get; private set; }

    private void Awake()
    {
        _board = Instantiate(_boardPrefab, Vector3.zero, Quaternion.identity);

        _player1 = new Player(_board, 1);
        _player2 = new Player(_board, 2);

        var opponentAI = new ChessAI(_player2, _player1, false);

        _player1.OnTurnComplete += _onTurnComplete;
        _player2.OnTurnComplete += _onTurnComplete;
        _board.OnSquareClicked += _placePlayerPiece;

        _roundNumber = 0;

        _initPlayerPieces();
        _transitionToState(GameState.PreRound);
    }

    private void OnDestroy()
    {
        _player1.OnTurnComplete -= _onTurnComplete;
        _player2.OnTurnComplete -= _onTurnComplete;
        _board.OnSquareClicked -= _placePlayerPiece;
    }

    private void Update()
    {
        if (GameState == GameState.Round)
        {
            _player1.Tick();
            _player2.Tick();

            if (_player1.ActivePieces.Count == 0)
            {
                _transitionToState(GameState.GameOver);
            }

            if (_player2.ActivePieces.Count == 0)
            {
                _transitionToState(GameState.Shop);
            }
        }

        //temp
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _transitionToState(GameState.PreRound);
        }
    }

    private void _initPlayerPieces()
    {
        _player1Pieces = new List<PieceData>
        {
            new PieceData(PieceType.Pawn, 1),
            new PieceData(PieceType.Pawn, 1)
        };

        _spawnedPlayerPieces = new List<ChessPiece>();
    }

    private void _onTurnComplete()
    {
        Turn++;

        var turn = Turn % 2;

        if (_startingPlayer == 1)
        {
            if (turn == 1)
            {
                _player1.SetPlayerTurn();
            }
            else if (turn == 0)
            {
                _player2.SetPlayerTurn();
            }
        }
        else if (_startingPlayer == 2)
        {
            if (turn == 1)
            {
                _player2.SetPlayerTurn();
            }
            else if (turn == 0)
            {
                _player1.SetPlayerTurn();
            }
        }
    }

    private void _spawnOpponentPieces(Color pieceColor)
    {
        int totalMaterialValue = _roundNumber;
        List<ChessPiece> spawnedPieces = new List<ChessPiece>();
        
        while (totalMaterialValue > 0)
        {
            var availableStartingSquares = _board.GetAvailableStartingSquares(2);

            if (availableStartingSquares.Count == 0)
            {
                Debug.LogError("Error: No Available Starting Squares Found To Spawn A Piece");
                break;
            }

            PieceType randomPieceType = (PieceType)Random.Range(0, 5);
            var randomSquare = availableStartingSquares[Random.Range(0, availableStartingSquares.Count)];

            switch (randomPieceType)
            {
                case PieceType.Pawn:

                    //replace the flat values with the Material Value of the piece
                    if (totalMaterialValue >= 1)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, 1);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, pieceColor, _player2));
                        
                        totalMaterialValue -= 1;
                    }

                    break;

                case PieceType.Knight:

                    if (totalMaterialValue >= 3)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, 3);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, pieceColor, _player2));

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Bishop:

                    if (totalMaterialValue >= 3)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, 3);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, pieceColor, _player2));

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Rook:

                    if (totalMaterialValue >= 5)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, 5);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, pieceColor, _player2));

                        totalMaterialValue -= 5;
                    }

                    break;

                case PieceType.Queen:

                    if (totalMaterialValue >= 9)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, 9);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, pieceColor, _player2));

                        totalMaterialValue -= 9;
                    }

                    break;
            }
        }
        
        _player2.SetPieces(spawnedPieces);
    }

    private void _placePlayerPiece(Square square)
    {
        if (GameState == GameState.PreRound)
        {
            var availableStartingSquares = _board.GetAvailableStartingSquares(1);

            if (!availableStartingSquares.Contains(square))
            {
                Debug.LogError("Error: Selected square is not an available starting square");
                return;
            }
            
            if (_startingPlayer == 1)
            {
                _spawnedPlayerPieces.Add(_board.SpawnPiece(_player1Pieces[_currentPiecePlacementIndex], square, Color.white, _player1));
            }
            else
            {
                _spawnedPlayerPieces.Add(_board.SpawnPiece(_player1Pieces[_currentPiecePlacementIndex], square, Color.black, _player1));
            }
            
            _currentPiecePlacementIndex++;

            if (_currentPiecePlacementIndex >= _player1Pieces.Count)
            {
                _player1.SetPieces(_spawnedPlayerPieces);
                _transitionToState(GameState.Round);
                return;
            }
            
            print($"Current Piece To Place: {_player1Pieces[_currentPiecePlacementIndex].PieceType}");
        }
    }

    private void _transitionToState(GameState state)
    {
        _endCurrentState();

        GameState = state;
        
        _initState();
    }

    private void _endCurrentState()
    {
        switch (GameState)
        {
            case GameState.PreRound:
                
                if (_startingPlayer == 1)
                {
                    _spawnOpponentPieces(Color.black);

                    _player1.SetPlayerTurn();
                }
                else
                {
                    _spawnOpponentPieces(Color.white);

                    _player2.SetPlayerTurn();
                }
                
                break;
            
            case GameState.Round:
                
                
                
                break;
            
            case GameState.Shop:
                
                
                
                break;
            
            default:

                print("No State To End");

                break;
        }
    }

    private void _initState()
    {
        switch (GameState)
        {
            case GameState.PreRound:
                
                _board.ClearBoard();
                _player2.ClearPieces();
                _player1.ClearPieces();
                
                _roundNumber++;
                Turn = 1;
                _spawnedPlayerPieces.Clear();
                _currentPiecePlacementIndex = 0;
                print($"Current Piece To Place: {_player1Pieces[_currentPiecePlacementIndex].PieceType}");
                
                _startingPlayer = Random.Range(1, 3);
                
                break;
            
            case GameState.Round:
                
                
                
                break;
            
            case GameState.Shop:
                
                print("Round Won");
                
                //TEMP
                _player1Pieces.Add(new PieceData(PieceType.Pawn, 1));
                _transitionToState(GameState.PreRound);
                ///////
                
                break;
            
            case GameState.GameOver:

                print("You Lose");

                break;
        }
    }
}

public enum GameState
{
    Default,
    PreRound,
    Round,
    Shop,
    GameOver
}
