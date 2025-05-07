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

    [SerializeField] 
    private MinimaxAI _aiPrefab;

    private ChessBoard _board;
    private MinimaxAI _ai;
    private Player _player1;
    private Player _player2;
    //private ChessAI _aiPlayer;
    private int _roundNumber;
    private List<PieceData> _player1Pieces;
    private List<PieceData> _player2Pieces;
    private int _currentPiecePlacementIndex;
    private int _startingPlayer;
    private List<ChessPiece> _spawnedPlayerPieces;

    public int Turn { get; private set; }

    public static GameState GameState { get; private set; }

    private void Awake()
    {
        _board = Instantiate(_boardPrefab, Vector3.zero, Quaternion.identity);
        _ai = Instantiate(_aiPrefab, Vector3.zero, Quaternion.identity);

        _player1 = new Player(_board, 1);
        _player2 = new Player(_board, 2);
        //_aiPlayer = new ChessAI(_player1, false);

        _player1.OnTurnComplete += _onTurnComplete;
        _player2.OnTurnComplete += _onTurnComplete;
        //_aiPlayer.OnTurnComplete += _onTurnComplete;
        //_board.OnSquareClicked += _placePlayerPiece;

        _roundNumber = 0;

        //_transitionToState(GameState.PreRound);
        _spawnDefaultStartingPieces();
        //_ai.Init(_board);
        _transitionToState(GameState.Round);
        //_initPlayerPieces();
    }

    private void OnDestroy()
    {
        _player1.OnTurnComplete -= _onTurnComplete;
        _player2.OnTurnComplete -= _onTurnComplete;
        //_aiPlayer.OnTurnComplete -= _onTurnComplete;
        //_board.OnSquareClicked -= _placePlayerPiece;
    }

    private void Update()
    {
        if (GameState == GameState.Round)
        {
            _player1.Tick();
            _player2.Tick();

            /*if (_player1.ActivePieces.Count == 0)
            {
                _transitionToState(GameState.GameOver);
            }

            if (_aiPlayer.ActivePieces.Count == 0)
            {
                _transitionToState(GameState.Shop);
            }*/
        }

        //temp
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            _transitionToState(GameState.PreRound);
        }*/
    }

    private void _spawnDefaultStartingPieces()
    {
        var whitePieces = new List<ChessPiece>();
        var blackPieces = new List<ChessPiece>();
        
        //Creating the white pieces
        for (int i = 1; i <= 8; i++)
        {
            whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Pawn, Color.white), _board.Board[1, i - 1], _player1));
        }

        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Bishop, Color.white), _board.Board[0, 2], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Bishop, Color.white), _board.Board[0, 5], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Knight, Color.white), _board.Board[0, 1], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Knight, Color.white), _board.Board[0, 6], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Rook, Color.white), _board.Board[0, 0], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Rook, Color.white), _board.Board[0, 7], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.Queen, Color.white), _board.Board[0, 3], _player1));
        whitePieces.Add(_board.SpawnPiece(new PieceData(PieceType.King, Color.white), _board.Board[0, 4], _player1));
        
        _player1.SetPieces(whitePieces);
        _player1Pieces = new List<PieceData>();

        foreach (var piece in whitePieces)
        {
            _player1Pieces.Add(piece.PieceData);
        }

        //black pieces
        for (int i = 1; i <= 8; i++)
        {
            blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Pawn, Color.black), _board.Board[6, i - 1], _player2));
        }

        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Bishop, Color.black), _board.Board[7, 2], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Bishop, Color.black), _board.Board[7, 5], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Knight, Color.black), _board.Board[7, 1], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Knight, Color.black), _board.Board[7, 6], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Rook, Color.black), _board.Board[7, 0], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Rook, Color.black), _board.Board[7, 7], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.Queen, Color.black), _board.Board[7, 3], _player2));
        blackPieces.Add(_board.SpawnPiece(new PieceData(PieceType.King, Color.black), _board.Board[7, 4], _player2));
        
        _player2.SetPieces(blackPieces);
        _player2Pieces = new List<PieceData>();

        foreach (var piece in blackPieces)
        {
            _player2Pieces.Add(piece.PieceData);
        }
    }

    /*private void _initPlayerPieces()
    {
        if (_startingPlayer == 1)
        {
            _player1Pieces = new List<PieceData>
            {
                new PieceData(PieceType.Pawn, Color.white),
                new PieceData(PieceType.Pawn, Color.white)
            };
        }
        else
        {
            _player1Pieces = new List<PieceData>
            {
                new PieceData(PieceType.Pawn, Color.black),
                new PieceData(PieceType.Pawn, Color.black)
            };
        }

        _spawnedPlayerPieces = new List<ChessPiece>();
    }*/

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
                //await _aiPlayer.SetAiTurn();
            }
        }
        else if (_startingPlayer == 2)
        {
            if (turn == 1)
            {
                _player2.SetPlayerTurn();
                //await _aiPlayer.SetAiTurn();
            }
            else if (turn == 0)
            {
                _player1.SetPlayerTurn();
            }
        }
    }

    /*private void _spawnOpponentPieces(Color pieceColor)
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
                        PieceData pieceData = new PieceData(randomPieceType, pieceColor);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, _aiPlayer));
                        
                        totalMaterialValue -= 1;
                    }

                    break;

                case PieceType.Knight:

                    if (totalMaterialValue >= 3)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, pieceColor);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, _aiPlayer));

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Bishop:

                    if (totalMaterialValue >= 3)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, pieceColor);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, _aiPlayer));

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Rook:

                    if (totalMaterialValue >= 5)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, pieceColor);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, _aiPlayer));

                        totalMaterialValue -= 5;
                    }

                    break;

                case PieceType.Queen:

                    if (totalMaterialValue >= 9)
                    {
                        PieceData pieceData = new PieceData(randomPieceType, pieceColor);
                        spawnedPieces.Add(_board.SpawnPiece(pieceData, randomSquare, _aiPlayer));

                        totalMaterialValue -= 9;
                    }

                    break;
            }
        }
        
        _aiPlayer.SetPieces(spawnedPieces);
    }*/

    /*private void _placePlayerPiece(Square square)
    {
        if (GameState == GameState.PreRound)
        {
            var availableStartingSquares = _board.GetAvailableStartingSquares(1);

            if (!availableStartingSquares.Contains(square))
            {
                Debug.LogError("Error: Selected square is not an available starting square");
                return;
            }

            _spawnedPlayerPieces.Add(_board.SpawnPiece(_player1Pieces[_currentPiecePlacementIndex], square, _player1));
            _currentPiecePlacementIndex++;

            if (_currentPiecePlacementIndex >= _player1Pieces.Count)
            {
                _player1.SetPieces(_spawnedPlayerPieces);
                _transitionToState(GameState.Round);
                return;
            }
            
            print($"Current Piece To Place: {_player1Pieces[_currentPiecePlacementIndex].PieceType}");
        }
    }*/

    private void _transitionToState(GameState state)
    {
        _endCurrentState();

        GameState = state;
        
        _initState();
    }

    private async void _endCurrentState()
    {
        switch (GameState)
        {
            case GameState.PreRound:
                
                /*if (_startingPlayer == 1)
                {
                    _spawnOpponentPieces(Color.black);

                    _player1.SetPlayerTurn();
                }
                else
                {
                    _spawnOpponentPieces(Color.white);

                    await _aiPlayer.SetAiTurn();
                }*/
                
                break;
            
            case GameState.Round:
                
                
                
                break;
            
            case GameState.Shop:
                
                
                
                break;
            
            default:

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
                
                Turn = 1;
                _player1.SetPlayerTurn();
                _startingPlayer = 1;
                _player2.SetAIPlayer(_ai, 1, false);
                
                break;
            
            case GameState.Shop:
                
                print("Round Won");
                
                //TEMP
                /*if (_startingPlayer == 1)
                {
                    _player1Pieces.Add(new PieceData(PieceType.Pawn, Color.white));
                }
                else
                {
                    _player1Pieces.Add(new PieceData(PieceType.Pawn, Color.black));   
                }*/

                _transitionToState(GameState.Round);
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
