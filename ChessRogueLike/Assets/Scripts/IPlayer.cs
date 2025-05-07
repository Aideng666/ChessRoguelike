using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using UnityEngine;

public interface IPlayer
{
    public void SetPieces(List<ChessPiece> pieces);
    public void RemovePiece(ChessPiece piece);
    public void PromotePiece(ChessPiece oldPiece, ChessPiece newPiece);
    public void ClearPieces();
    public void AddTakenPiece(ChessPiece piece);
}
