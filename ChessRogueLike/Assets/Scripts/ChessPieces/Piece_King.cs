using UnityEngine;

namespace ChessPieces
{
    public class Piece_King : ChessPiece
    {
        public override void Init(Square startSquare, IPlayer player, PieceData pieceData)
        {
            base.Init(startSquare, player, pieceData);

            _pieceType = PieceType.King;
        }

        private void _checkAvailableAdjacentSquare(Direction direction)
        {
            var squareToCheck = _board.GetAdjacentSquare(CurrentSquare, direction);

            if (squareToCheck != null && squareToCheck.CurrentPiece == null)
            {
                AvailableSquares.Add(squareToCheck);
            }
            else if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.PieceData.Color != PieceData.Color)
            {
                AvailableSquares.Add(squareToCheck);
            }
        }

        protected override void _checkAvailableSquares()
        {
            base._checkAvailableSquares();

            _checkAvailableAdjacentSquare(Direction.North);
            _checkAvailableAdjacentSquare(Direction.East);
            _checkAvailableAdjacentSquare(Direction.South);
            _checkAvailableAdjacentSquare(Direction.West);
            _checkAvailableAdjacentSquare(Direction.NorthEast);
            _checkAvailableAdjacentSquare(Direction.NorthWest);
            _checkAvailableAdjacentSquare(Direction.SouthEast);
            _checkAvailableAdjacentSquare(Direction.SouthWest);
        }
    }
}
