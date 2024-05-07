using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ChessBoard _boardPrefab;

    private ChessBoard _board;
    private Player _player1;
    private Player _player2;
    private GameState _gameState;
    private int _roundNumber;

    public int Turn { get; private set; }

    private void Awake()
    {
        _board = Instantiate(_boardPrefab, Vector3.zero, Quaternion.identity);

        _player1 = new Player(_board);
        _player2 = new Player(_board);

        _player1.OnTurnComplete += _onTurnComplete;
        _player2.OnTurnComplete += _onTurnComplete;

        _roundNumber = 0;

        _startNewRound();
    }

    private void OnDestroy()
    {
        _player1.OnTurnComplete -= _onTurnComplete;
        _player2.OnTurnComplete -= _onTurnComplete;
    }

    private void Update()
    {
        if (_gameState == GameState.Round)
        {
            _player1.Tick();
            _player2.Tick();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startNewRound();
        }
    }

    private void _onTurnComplete()
    {
        Turn++;

        var turn = Turn % 2;

        if (turn == 1)
        {
            _player1.SetPlayerTurn();
        }
        else if (turn == 0)
        {
            _player2.SetPlayerTurn();
        }
    }

    private void _startNewRound()
    {
        _board.ClearBoard();
        _player1.ClearPieces();
        _player2.ClearPieces();

        _gameState = GameState.PreRound;
        _roundNumber++;
        Turn = 1;

        var randomStartingPlayer = Random.Range(1, 3);

        if (randomStartingPlayer == 1)
        {
            _spawnOpponentPieces(Color.black);

            _player1.SetPlayerTurn();
        }
        else
        {
            _spawnOpponentPieces(Color.white);

            _player2.SetPlayerTurn();
        }
    }

    private void _spawnOpponentPieces(Color pieceColor)
    {
        int totalMaterialValue = _roundNumber;

        while (totalMaterialValue > 0)
        {
            var availableStartingSquares = _board.GetAvailableStartingSquares(2);

            if (availableStartingSquares.Count == 0)
            {
                Debug.LogError("Error: No Available Starting Squares Found To Spawn A Piece");
                break;
            }

            PieceType randomPiece = (PieceType)Random.Range(0, 5);
            var randomSquare = availableStartingSquares[Random.Range(0, availableStartingSquares.Count)];

            switch (randomPiece)
            {
                case PieceType.Pawn:

                    //replace the flat values with the Material Value of the piece
                    if (totalMaterialValue >= 1)
                    {
                        _board.SpawnPiece(randomPiece, randomSquare, pieceColor);

                        totalMaterialValue -= 1;
                    }

                    break;

                case PieceType.Knight:

                    if (totalMaterialValue >= 3)
                    {
                        _board.SpawnPiece(randomPiece, randomSquare, pieceColor);

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Bishop:

                    if (totalMaterialValue >= 3)
                    {
                        _board.SpawnPiece(randomPiece, randomSquare, pieceColor);

                        totalMaterialValue -= 3;
                    }

                    break;

                case PieceType.Rook:

                    if (totalMaterialValue >= 5)
                    {
                        _board.SpawnPiece(randomPiece, randomSquare, pieceColor);

                        totalMaterialValue -= 5;
                    }

                    break;

                case PieceType.Queen:

                    if (totalMaterialValue >= 9)
                    {
                        _board.SpawnPiece(randomPiece, randomSquare, pieceColor);

                        totalMaterialValue -= 9;
                    }

                    break;
            }
        }
    }
}

public enum GameState
{
    PreRound,
    Round,
    Shop
}
