using System.Collections.Generic;
using UnityEngine;

namespace ChessPieces
{
    public class ChessPiece : MonoBehaviour
    {
        [SerializeField] private Sprite _whiteSprite;
        [SerializeField] private Sprite _blackSprite;

        protected PieceType _pieceType;
        protected ChessBoard _board;
        //protected Player _owningPlayer;
        private SpriteRenderer _spriteRenderer;

        protected int _numberOfMovesMade = 0;

        public List<Square> AvailableSquares { get; private set; }
        public Color Color { get; private set; }
        public Square CurrentSquare { get; private set; }
        public PieceData PieceData { get; private set; }
        public Player OwningPlayer { get; private set; }

        public virtual void Init(Square startSquare, Color color, Player owningPlayer, PieceData pieceData)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _board = FindObjectOfType<ChessBoard>();
            Color = color;
            AvailableSquares = new List<Square>();
            CurrentSquare = startSquare;
            CurrentSquare.SetCurrentPiece(this);
            transform.position = startSquare.transform.position;
            _numberOfMovesMade = 0;
            OwningPlayer = owningPlayer;
            PieceData = pieceData;
            _checkAvailableSquares();

            if (Color == Color.black)
            {
                _spriteRenderer.sprite = _blackSprite;
            }
            else
            {
                _spriteRenderer.sprite = _whiteSprite;
            }

        }

        private void OnDestroy()
        {
            _spriteRenderer.enabled = false;
        }

        public virtual void MoveTo(Square square)
        {
            var takenPiece = square.CurrentPiece;
            
            if (takenPiece != null)
            {
                OwningPlayer.AddTakenPiece(takenPiece);
                takenPiece.OwningPlayer.RemovePiece(takenPiece);
            }

            CurrentSquare.SetCurrentPiece(null);
            transform.position = square.transform.position;
            CurrentSquare = square;
            CurrentSquare.SetCurrentPiece(this);
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
}