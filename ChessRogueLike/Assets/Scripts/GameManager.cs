using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ChessBoard _boardPrefab;

    private Player _player1;
    private Player _player2;

    public int Turn { get; private set; }

    private void Awake()
    {
        var board = Instantiate(_boardPrefab, Vector3.zero, Quaternion.identity);

        _player1 = new Player(board, board.WhitePieces);
        _player2 = new Player(board, board.BlackPieces);

        _player1.OnTurnComplete += _onTurnComplete;
        _player2.OnTurnComplete += _onTurnComplete;

        Turn = 1;

        _player1.SetPlayerTurn();
    }

    private void OnDestroy()
    {
        _player1.OnTurnComplete -= _onTurnComplete;
        _player2.OnTurnComplete -= _onTurnComplete;
    }

    private void Update()
    {
        _player1.Tick();
        _player2.Tick();
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
}
