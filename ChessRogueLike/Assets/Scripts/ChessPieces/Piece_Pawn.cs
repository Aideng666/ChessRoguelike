using UnityEngine;

namespace ChessPieces
{
    public class Piece_Pawn : ChessPiece
    {
        private int _finalRank;
        
        public override void Init(Square startSquare, IPlayer player, PieceData pieceData)
        {
            base.Init(startSquare, player, pieceData);

            _pieceType = PieceType.Pawn;

            if (player.GetType() == typeof(Player))
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

            if (OwningPlayer.GetType() == typeof(Player))
            {
                _checkAvailableSquaresForPlayer1();
            }
            else if (OwningPlayer.GetType() == typeof(ChessAI))
            {
                _checkAvailableSquaresForPlayer2();
            }
        }

        private void _promotePiece()
        {
            //TODO: This defaults to queen for now, update so it can be chosen
            PieceData pieceData = new PieceData(PieceType.Queen, PieceData.Color);
            var piece = _board.SpawnPiece(pieceData, CurrentSquare, OwningPlayer);

            OwningPlayer.PromotePiece(this, piece);
            
            //Destroys because we promoted and created a new piece game object
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

            if (squareToCheck != null && squareToCheck.CurrentPiece != null && squareToCheck.CurrentPiece.PieceData.Color != PieceData.Color)
            {
                AvailableSquares.Add(squareToCheck);
            }
        }
    }
}
