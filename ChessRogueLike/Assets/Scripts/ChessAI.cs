using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessPieces;
using Codice.CM.Client.Differences.Merge;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChessAI : IPlayer
{
    public List<ChessPiece> ActivePieces { get; private set; }
    
    public Action OnTurnComplete;
    
    private Player _opponent;
    private bool _isBossAI;
    private bool _hasTurn;

    private List<ChessPiece> _takenOpponentPieces;
    public ChessAI(Player opponent, bool isBoss)
    {
        _opponent = opponent;
        _isBossAI = isBoss;
        _hasTurn = false;
        
        ActivePieces = new List<ChessPiece>();
        _takenOpponentPieces = new List<ChessPiece>();
    }

    //for now this will only check its own available moves and pick the one that gives it the highest gain of material
    public ChessMove FindBestMove(int depth)
    {
        var legalMoves = _getAllLegalMoves();

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
    
    public void SetPieces(List<ChessPiece> pieces)
    {
        ActivePieces = pieces;
    }
    
    public void RemovePiece(ChessPiece piece)
    {
        if (ActivePieces.Contains(piece))
        {
            ActivePieces.Remove(piece);
            GameObject.Destroy(piece.gameObject);
        }
    }
    
    public void PromotePiece(ChessPiece oldPiece, ChessPiece newPiece)
    {
        ActivePieces.Remove(oldPiece);
        ActivePieces.Add(newPiece);
    }

    public void ClearPieces()
    {
        ActivePieces.Clear();
    }

    public void AddTakenPiece(ChessPiece piece)
    {
        _takenOpponentPieces.Add(piece);
    }


    public async Task SetAiTurn()
    {
        _hasTurn = true;

        await PlayBestMove();
    }
    
    private async Task PlayBestMove()
    {
        await Task.Delay(1000);

        var move = FindBestMove(1);

        move?.PieceToMove.MoveTo(move.TargetSquare);

        OnTurnComplete?.Invoke();
    }

    private List<ChessMove> _getAllLegalMoves()
    {
        var legalMoves = new List<ChessMove>();

        foreach (var piece in ActivePieces)
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