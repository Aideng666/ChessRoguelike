using UnityEngine;

namespace ChessPieces
{
    public class Piece_Queen : ChessPiece
    {
        public override void Init(Square startSquare, IPlayer player, PieceData pieceData)
        {
            base.Init(startSquare, player, pieceData);

            _pieceType = PieceType.Queen;
        }

        private void _checkAvailableSquaresInDirection(Direction direction)
        {
            var squaresToCheck = _board.GetSquaresInDirection(CurrentSquare, direction);

            if (squaresToCheck.Count > 0)
            {
                foreach (var square in squaresToCheck)
                {
                    if (square.CurrentPiece != null && square.CurrentPiece.PieceData.Color == PieceData.Color)
                    {
                        break;
                    }
                    else if (square.CurrentPiece != null && square.CurrentPiece.PieceData.Color != PieceData.Color)
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
            _checkAvailableSquaresInDirection(Direction.NorthEast);
            _checkAvailableSquaresInDirection(Direction.NorthWest);
            _checkAvailableSquaresInDirection(Direction.SouthEast);
            _checkAvailableSquaresInDirection(Direction.SouthWest);
        }
    }
}
