using UnityEngine;

namespace ChessPieces
{
    public class Piece_Knight : ChessPiece
    {
        public override void Init(Square startSquare, Color color, Player player, PieceData pieceData)
        {
            base.Init(startSquare, color, player, pieceData);

            _pieceType = PieceType.Knight;
        }

        private void _checkAvailableKnightSquare(KnightDirection direction)
        {
            var squareToCheck = _board.CheckSquareKnightSpaceAway(CurrentSquare, direction);

            if (squareToCheck != null && squareToCheck.CurrentPiece == null)
            {
                AvailableSquares.Add(squareToCheck);
            }
            else if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.Color != Color)
            {
                AvailableSquares.Add(squareToCheck);
            }
        }

        protected override void _checkAvailableSquares()
        {
            base._checkAvailableSquares();

            _checkAvailableKnightSquare(KnightDirection.OneOClock);
            _checkAvailableKnightSquare(KnightDirection.TwoOClock);
            _checkAvailableKnightSquare(KnightDirection.FourOClock);
            _checkAvailableKnightSquare(KnightDirection.FiveOClock);
            _checkAvailableKnightSquare(KnightDirection.SevenOClock);
            _checkAvailableKnightSquare(KnightDirection.EightOClock);
            _checkAvailableKnightSquare(KnightDirection.TenOClock);
            _checkAvailableKnightSquare(KnightDirection.ElevenOCLock);
        }
    }
}
