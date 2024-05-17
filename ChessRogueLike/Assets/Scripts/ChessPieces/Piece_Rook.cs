using UnityEngine;

namespace ChessPieces
{
    public class Piece_Rook : ChessPiece
    {
        public override void Init(Square startSquare, Color color, Player player, PieceData pieceData)
        {
            base.Init(startSquare, color, player, pieceData);

            _pieceType = PieceType.Rook;
        }

        private void _checkAvailableSquaresInDirection(Direction direction)
        {
            var squaresToCheck = _board.GetSquaresInDirection(CurrentSquare, direction);

            if (squaresToCheck.Count > 0)
            {
                foreach (var square in squaresToCheck)
                {
                    if (square.CurrentPiece != null && square.CurrentPiece.Color == Color)
                    {
                        break;
                    }
                    else if (square.CurrentPiece != null && square.CurrentPiece.Color != Color)
                    {
                        AvailableSquares.Add(square);
                        break;
                    }

                    AvailableSquares.Add(square);
                }
            }
        }

        protected override void _checkAvailableSquares()
        {
            base._checkAvailableSquares();

            _checkAvailableSquaresInDirection(Direction.North);
            _checkAvailableSquaresInDirection(Direction.East);
            _checkAvailableSquaresInDirection(Direction.South);
            _checkAvailableSquaresInDirection(Direction.West);
        }
    }
}
