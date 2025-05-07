using System;
using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using UnityEngine;

public class MinimaxAI : MonoBehaviour
{
    private char[,] _board = new char[8,8];
    private ChessBoard _chessboard;

    /*public void Init(ChessBoard board)
    {
       
    }*/

    /*private PieceData getPieceFromChar(char pieceChar)
    {
        PieceData piece = null;

        switch (pieceChar)
        {
            case 'p':

                piece = new PieceData(PieceType.Pawn, 1, Color.black);
                
                break;
            
            case 'P':

                piece = new PieceData(PieceType.Pawn, 1, Color.white);                
                
                break;
            
            case 'k':

                piece = new PieceData(PieceType.King, 0, Color.black);
                
                break;
            
            case 'K':

                piece = new PieceData(PieceType.King, 0, Color.white);
                
                break;
            
            case 'n':

                piece = new PieceData(PieceType.Knight, 3, Color.black);
                
                break;
            
            case 'N':

                piece = new PieceData(PieceType.Knight, 3, Color.white);
                
                break;
            
            case 'q':

                piece = new PieceData(PieceType.Queen, 9, Color.black);
                
                break;
            
            case 'Q':

                piece = new PieceData(PieceType.Queen, 9, Color.white);
                
                break;
            
            case 'r':

                piece = new PieceData(PieceType.Rook, 5, Color.black);
                
                break;
            
            case 'R':

                piece = new PieceData(PieceType.Rook, 5, Color.white);
                
                break;
            
            case 'b':

                piece = new PieceData(PieceType.Bishop, 3, Color.black);
                
                break;
            
            case 'B':

                piece = new PieceData(PieceType.Bishop, 3, Color.white);
                
                break;
        }

        return piece;
    }*/

    private char getCharFromPiece(ChessPiece piece)
    {
        char selectedChar = '.';

        if (piece == null)
        {
            return selectedChar;
        }

        switch (piece.PieceData.PieceType)
        {
            case PieceType.Pawn:

                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'p';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'P';
                }
                
                break;
            case PieceType.Knight:
                
                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'n';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'N';
                }
                
                break;
            case PieceType.Bishop:
                
                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'b';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'B';
                }
                
                break;
            case PieceType.Rook:
                
                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'r';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'R';
                }
                
                break;
            case PieceType.Queen:
                
                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'q';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'Q';
                }
                
                break;
            case PieceType.King:
                
                if (piece.PieceData.Color == Color.black)
                {
                    selectedChar = 'k';
                }
                else if (piece.PieceData.Color == Color.white)
                {
                    selectedChar = 'K';
                }
                
                break;
        }

        return selectedChar;
    }
    
    public Move GetBestMove(int depth, bool isWhite, ChessBoard board)
    {
        _chessboard = board;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _board[i, j] = getCharFromPiece(board.Board[i, j].CurrentPiece);
            }
        }
        
        printBoard(_board);
        
        List<int> bestEval = new List<int>();
        List<Move> bestMoves = new List<Move>();
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        foreach (Move move in GenerateMoves(_board, isWhite))
        {
            _board = makeMove(_board, move);
            int eval = MinimaxAB(_board, depth - 1, alpha, beta, false, isWhite);
            _board = undoMove(_board, move, isWhite);
            
            if (bestEval.Count < 1 || eval > bestEval[0])
            {
                bestEval.Clear();
                bestMoves.Clear();
                bestEval.Add(eval);
                bestMoves.Add(move);
            }
            else if (eval == bestEval[0])
            {
                bestEval.Add(eval);
                bestMoves.Add(move);
            }
        }

        bestMoves = ShuffleList(bestMoves);

        return bestMoves[0];
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
    }

    private List<Move> GenerateMoves(char[,] board, bool isWhite)
    {
        List<Move> legalMoves = new List<Move>();
        
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                char pieceChar = board[row, col];

                if (pieceChar == '.')
                {
                    //No Piece On This Square To Check
                    continue;
                }

                if ((isWhite && char.IsUpper(pieceChar)) || (!isWhite && char.IsLower(pieceChar)))
                {
                    switch (char.ToLower(pieceChar))
                    {
                        case 'p': legalMoves.AddRange(GetPossiblePawnMoves(board, row, col, isWhite)); break;
                        case 'n': legalMoves.AddRange(GetPossibleKnightMoves(board, row, col, isWhite)); break;
                        case 'b': legalMoves.AddRange(GetPossibleBishopMoves(board, row, col, isWhite)); break;
                        case 'r': legalMoves.AddRange(GetPossibleRookMoves(board, row, col, isWhite)); break;
                        case 'q': legalMoves.AddRange(GetPossibleQueenMoves(board, row, col, isWhite)); break;
                        case 'k': legalMoves.AddRange(GetPossibleKingMoves(board, row, col, isWhite)); break;
                    }
                }
            }
        }
        
        return legalMoves;
    }

    private bool IsInBoard(int row, int col)
    {
        return row < 8 && row >= 0 && col < 8 && col >= 0;
    }

    private List<Move> GetPossiblePawnMoves(char[,] board, int row, int col, bool isWhite)
    {
        var moves = new List<Move>();
        
        int direction = isWhite ? 1 : -1;

        int newRow = row + direction;
        int doubleMoveRow = row + (direction * 2);
        
        //Forward
        if (IsInBoard(newRow, col) && board[newRow, col] == '.')
        {
            var move = new Move
            {
                MovedPiece = board[row, col],
                CapturedPiece = '.',
                StartingSquare = new Vector2Int(row, col),
                EndingSquare = new Vector2Int(newRow, col)
            };

            if (!isWhite && newRow == 0)
            {
                move.IsPromotion = true;
                move.PromotionPiece = 'n';
                moves.Add(move);

                move.PromotionPiece = 'b';
                moves.Add(move);

                move.PromotionPiece = 'r';
                moves.Add(move);

                move.PromotionPiece = 'q';
                moves.Add(move);

            }
            else if (isWhite && newRow == 7)
            {
                move.IsPromotion = true;
                move.PromotionPiece = 'N';
                moves.Add(move);

                move.PromotionPiece = 'B';
                moves.Add(move);

                move.PromotionPiece = 'R';
                moves.Add(move);

                move.PromotionPiece = 'Q';
                moves.Add(move);
            }
            else
            {
                moves.Add(move);
            }
        }

        if (IsInBoard(doubleMoveRow, col) && board[newRow, col] == '.' && board[doubleMoveRow, col] == '.')
        {
            //TODO: Make sure the piece hasn't moved yet somehow?
            if ((isWhite && row <= 1) || (!isWhite && row >= 6))
            {
                var move = new Move
                {
                    MovedPiece = board[row, col],
                    CapturedPiece = '.',
                    StartingSquare = new Vector2Int(row, col),
                    EndingSquare = new Vector2Int(doubleMoveRow, col)
                };
                
               moves.Add(move);
            }
        }

        // Capture diagonally
        if (IsInBoard(newRow, col - 1) && board[newRow, col - 1] != '.' && char.IsUpper(board[newRow, col - 1]) != isWhite)
        {
            var move = new Move
            {
                MovedPiece = board[row, col],
                CapturedPiece = board[newRow, col - 1],
                StartingSquare = new Vector2Int(row, col),
                EndingSquare = new Vector2Int(newRow, col - 1)
            };
                
            moves.Add(move);
        }
        if (IsInBoard(newRow, col + 1) && board[newRow, col + 1] != '.' && char.IsUpper(board[newRow, col + 1]) != isWhite)
        {
            var move = new Move
            {
                MovedPiece = board[row, col],
                CapturedPiece = board[newRow, col + 1],
                StartingSquare = new Vector2Int(row, col),
                EndingSquare = new Vector2Int(newRow, col + 1)
            };
                
            moves.Add(move);
        }

        return moves;
    }

    private List<Move> GetPossibleKnightMoves(char[,] board, int row, int col, bool isWhite)
    {
        var moves = new List<Move>();
        Vector2Int[] knightMoves = { new(2,1), new(2,-1), new(-2,1), new(-2,-1),
            new(1,2), new(1,-2), new(-1,2), new(-1,-2) };

        foreach (var knightMove in knightMoves)
        {
            int newRow = row + knightMove[0];
            int newCol = col + knightMove[1];

            if (IsInBoard(newRow, newCol) && (board[newRow, newCol] == '.' || char.IsUpper(board[newRow, newCol]) != isWhite))
            {
                var move = new Move
                {
                    MovedPiece = board[row, col],
                    CapturedPiece = board[newRow, newCol],
                    StartingSquare = new Vector2Int(row, col),
                    EndingSquare = new Vector2Int(newRow, newCol)
                };
                
                moves.Add(move);
            }
        }
    
        return moves;
    }

    private List<Move> GetPossibleKingMoves(char[,] board, int row, int col, bool isWhite)
    {
        var moves = new List<Move>();
        Vector2Int[] kingMoves = { new(1, 1), new(-1, -1), new(-1, 1), new(1, -1),
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        
        foreach (var kingMove in kingMoves)
        {
            int newRow = row + kingMove[0];
            int newCol = col + kingMove[1];

            if (IsInBoard(newRow, newCol) && (board[newRow, newCol] == '.' || char.IsUpper(board[newRow, newCol]) != isWhite))
            {
                var move = new Move
                {
                    MovedPiece = board[row, col],
                    CapturedPiece = board[newRow, newCol],
                    StartingSquare = new Vector2Int(row, col),
                    EndingSquare = new Vector2Int(newRow, newCol)
                };
                
                moves.Add(move);
            }
        }
    
        return moves;
    }

    private List<Move> GetPossibleSlidingMoves(char[,] board, int row, int col, bool isWhite, Vector2Int[] directions)
    {
        List<Move> moves = new List<Move>();

        foreach (var dir in directions)
        {
            int newRow = row;
            int newCol = col;

            while (true)
            {
                newRow += dir[0];
                newCol += dir[1];

                if (!IsInBoard(newRow, newCol))
                {
                    break;
                }
                
                
                if (board[newRow, newCol] == '.')
                {
                    var move = new Move
                    {
                        MovedPiece = board[row, col],
                        CapturedPiece = board[newRow, newCol],
                        StartingSquare = new Vector2Int(row, col),
                        EndingSquare = new Vector2Int(newRow, newCol)
                    };
                    
                    moves.Add(move);
                }
                else
                {
                    if (char.IsUpper(board[newRow, newCol]) != isWhite)
                    {
                        var move = new Move
                        {
                            MovedPiece = board[row, col],
                            CapturedPiece = board[newRow, newCol],
                            StartingSquare = new Vector2Int(row, col),
                            EndingSquare = new Vector2Int(newRow, newCol)
                        };
                    
                        moves.Add(move);
                    }
                    
                    break;
                }
            }
        }

        return moves;
    }

    private List<Move> GetPossibleBishopMoves(char[,] board, int row, int col, bool isWhite)
    {
        Vector2Int[] directions = { new(1, 1), new(-1, -1), new(-1, 1), new(1, -1) };
        return GetPossibleSlidingMoves(board, row, col, isWhite, directions);
    }
    
    private List<Move> GetPossibleRookMoves(char[,] board, int row, int col, bool isWhite)
    {
        Vector2Int[] directions = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        return GetPossibleSlidingMoves(board, row, col, isWhite, directions);
    }
    
    private List<Move> GetPossibleQueenMoves(char[,] board, int row, int col, bool isWhite)
    {
        Vector2Int[] directions = { new(1, 1), new(-1, -1), new(-1, 1), new(1, -1),
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        return GetPossibleSlidingMoves(board, row, col, isWhite, directions);
    }
    
    private int MinimaxAB(char[,] board, int depth, int alpha, int beta, bool isMaximizing, bool isWhite)
    {
        if (depth == 0)
        {
            return EvaluateBoard(board, isWhite);
        }

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (Move move in GenerateMoves(board, true))
            {
                board = makeMove(board, move);
                int eval = MinimaxAB(board, depth - 1, alpha, beta, false, isWhite);
                board = undoMove(board, move, true);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha) break; // Prune
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (Move move in GenerateMoves(board, false))
            {
                board = makeMove(board, move);
                int eval = MinimaxAB(board, depth - 1, alpha, beta, true, isWhite);
                board = undoMove(board, move, false);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha) break; // Prune
            }
            return minEval;
        }
    }

    private char[,] makeMove(char[,] board, Move move)
    {
        board[move.StartingSquare.x, move.StartingSquare.y] = '.';
        board[move.EndingSquare.x, move.EndingSquare.y] = move.MovedPiece;

        if (move.IsPromotion)
        {
            board[move.EndingSquare.x, move.EndingSquare.y] = move.PromotionPiece;
        }

        return board;
    }
    
    private char[,] undoMove(char[,] board, Move move, bool isWhite)
    {
        board[move.StartingSquare.x, move.StartingSquare.y] = move.MovedPiece;
        board[move.EndingSquare.x, move.EndingSquare.y] = move.CapturedPiece;

        if (move.IsPromotion)
        {
            if (isWhite)
            {
                board[move.StartingSquare.x, move.StartingSquare.y] = 'P';
            }
            else
            {
                board[move.StartingSquare.x, move.StartingSquare.y] = 'p';
            }
        }

        return board;
    }
    
    int EvaluateBoard(char[,] board, bool isWhite)
    {
        int score = 0;

        foreach (var tile in board)
        {
            if (tile == '.')
            {
                continue;
            }

            if (isWhite)
            {
                if (char.IsUpper(tile))
                {
                    score += GetPieceValue(tile);
                }
                else
                {
                    score -= GetPieceValue(tile);
                }
            }
            else
            {
                if (char.IsUpper(tile))
                {
                    score -= GetPieceValue(tile);
                }
                else
                {
                    score += GetPieceValue(tile);
                }
            }
        }
        
        /*print("BOARD");
        printBoard(board);
        print("SCORE: " + score);*/

        return score;
    }

    private void printBoard(char[,] board)
    {
        string row = "";
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                row += board[i, j];
                row += " | ";
            }

            row += "\n";
        }
        
        print(row);
    }

    int GetPieceValue(char piece)
    {
        piece = piece.ToString().ToUpper().ToCharArray()[0];
        
        switch (piece)
        {
            case 'P': return 10;
            case 'N': return 30;
            case 'B': return 30;
            case 'R': return 50;
            case 'Q': return 90;
            case 'K': return 900;
            default: return 0;
        }
    }
}
