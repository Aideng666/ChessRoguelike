using UnityEngine;

namespace ChessPieces
{
    public class Piece_Pawn : ChessPiece
    {
        private int _finalRank;
        
        public override void Init(Square startSquare, Color color, Player player, PieceData pieceData)
        {
            base.Init(startSquare, color, player, pieceData);

            _pieceType = PieceType.Pawn;

            if (player.PlayerNum == 1)
            {
                _finalRank = 8;
            }
            else
            {
                _finalRank = 1;
            }
        }

        public override void MoveTo(Square square)
        {
            base.MoveTo(square);

            if (square.Rank == _finalRank)
            {
                _promotePiece();
            }
        }

        protected override void _checkAvailableSquares()
        {
            base._checkAvailableSquares();

            if (OwningPlayer.PlayerNum == 1)
            {
                _checkAvailableSquaresForPlayer1();
            }
            else if (OwningPlayer.PlayerNum == 2)
            {
                _checkAvailableSquaresForPlayer2();
            }
        }

        private void _promotePiece()
        {
            //default to queen for now
            PieceData pieceData = new PieceData(PieceType.Queen, 9);
            var piece = _board.SpawnPiece(pieceData, CurrentSquare, Color, OwningPlayer);
            
            OwningPlayer.PromotePiece(this, piece);
            
            Destroy(gameObject);
        }

        private void _checkAvailableSquaresForPlayer1()
        {
            //first checks one square ahead
            var squareToCheck = _board.GetAdjacentSquare(CurrentSquare, Direction.North);

            if (squareToCheck != null && squareToCheck.CurrentPiece == null)
            {
                AvailableSquares.Add(squareToCheck);

                //if the one ahead was available it also checks 2 squares ahead if its the first move its making
                if (_numberOfMovesMade == 0)
                {
                    squareToCheck = _board.GetAdjacentSquare(squareToCheck, Direction.North);

                    if (squareToCheck != null && squareToCheck.CurrentPiece == null)
                    {
                        AvailableSquares.Add(squareToCheck);
                    }
                }
            }

            _checkDiagonal(Direction.NorthEast);
            _checkDiagonal(Direction.NorthWest);

            //TODO: Check for en passant
        }

        private void _checkAvailableSquaresForPlayer2()
        {
            //first checks one square ahead
            var squareToCheck = _board.GetAdjacentSquare(CurrentSquare, Direction.South);

            if (squareToCheck != null && squareToCheck.CurrentPiece == null)
            {
                AvailableSquares.Add(squareToCheck);

                //if the one ahead was available it also checks 2 squares ahead if its the first move its making
                if (_numberOfMovesMade == 0)
                {
                    squareToCheck = _board.GetAdjacentSquare(squareToCheck, Direction.South);

                    if (squareToCheck != null && squareToCheck.CurrentPiece == null)
                    {
                        AvailableSquares.Add(squareToCheck);
                    }
                }
            }

            _checkDiagonal(Direction.SouthEast);
            _checkDiagonal(Direction.SouthWest);
        }

        private void _checkDiagonal(Direction direction)
        {
            var squareToCheck = _board.GetAdjacentSquare(CurrentSquare, direction);

            if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.Color != Color)
            {
                AvailableSquares.Add(squareToCheck);
            }
        }
    }
}
