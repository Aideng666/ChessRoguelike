using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] private Sprite _whiteSprite;
    [SerializeField] private Sprite _blackSprite;
    [SerializeField] private Image _pieceIcon;

    protected PieceType _pieceType;
    protected int _materialValue;
    protected Square _currentSquare;
    protected ChessBoard _board;

    protected int _numberOfMovesMade = 0;

    public List<Square> AvailableSquares { get; protected set; }
    public Color Color { get; private set; }

    public virtual void Init(Square startSquare, Color color)
    {
        _board = FindObjectOfType<ChessBoard>();
        Color = color;
        AvailableSquares = new List<Square>();
        _currentSquare = startSquare;
        _currentSquare.SetCurrentPiece(this);
        transform.position = startSquare.transform.position;
        _numberOfMovesMade = 0;
        _checkAvailableSquares();

        if (Color == Color.black)
        {
            _pieceIcon.sprite = _blackSprite;
        }
        else
        {
            _pieceIcon.sprite = _whiteSprite;
        }

    }

    private void OnDestroy()
    {
        _pieceIcon.enabled = false;
    }

    public void MoveTo(Square square)
    {
        if (square.CurrentPiece != null)
        {
            //replace with some sort of TakePiece function that keeps track of all pieces that were taken
            Destroy(square.CurrentPiece);
        }

        _currentSquare.SetCurrentPiece(null);
        transform.position = square.transform.position;
        _currentSquare = square;
        _currentSquare.SetCurrentPiece(this);
        _numberOfMovesMade++;
        _checkAvailableSquares();
    }

    public void UpdateAvailableSquares()
    {
        _checkAvailableSquares();
    }

    protected virtual void _checkAvailableSquares()
    {
        AvailableSquares.Clear();
    }
}

public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
