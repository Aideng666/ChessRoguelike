using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChessPieces;
using Codice.CM.Client.Differences.Merge;
using UnityEngine;

public class ChessAI
{
    private Player _player;
    private Player _opponent;
    private bool _isBossAI;
    
    public ChessAI(Player player, Player opponent, bool isBoss)
    {
        _player = player;
        _opponent = opponent;
        _isBossAI = isBoss;
        
        player.SetAI(this);
    }

    //for now this will only check its own available moves and pick the one that gives it the highest gain of material
    public ChessMove FindBestMove(int depth)
    {
        var legalMoves = _getAllLegalMoves(_player);

        var bestMoves = new List<ChessMove>();

        int highestMaterial = 0;
        
        foreach (var move in legalMoves)
        {
            if (move.MaterialGain > highestMaterial)
            {
                bestMoves.Clear();
                bestMoves.Add(move);
                highestMaterial = move.MaterialGain;
            }
            else if (move.MaterialGain == highestMaterial)
            {
                bestMoves.Add(move);
            }
        }

        if (bestMoves.Count > 0)
        {
            return bestMoves[Random.Range(0, bestMoves.Count)];
        }

        return null;
    }

    private List<ChessMove> _getAllLegalMoves(Player player)
    {
        var legalMoves = new List<ChessMove>();

        foreach (var piece in player.ActivePieces)
        {
            foreach (var square in piece.AvailableSquares)
            {
                int materialGain = 0;

                if (square.CurrentPiece != null)
                {
                    materialGain = square.CurrentPiece.PieceData.MaterialValue;
                }
                
                legalMoves.Add(new ChessMove
                {
                    PieceToMove = piece,
                    TargetPiece = square.CurrentPiece,
                    TargetSquare = square,
                    MaterialGain = materialGain
                });
            }
        }

        return legalMoves;
    }
}

public class ChessMove
{
    public ChessPiece PieceToMove;
    public Square TargetSquare;
    public ChessPiece TargetPiece;
    public int MaterialGain;
}