using System.Collections.Generic;
using UnityEngine;

public class ColorBomb : Piece, IBombPiece
{
    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> bombedPieces = new List<Piece>();
        return bombedPieces;
    }

    public void SetSelfColorFromAnotherPiece(Piece anotherPiece)
    {
        SetPieceColor(anotherPiece.GetPieceColor());
    }
}
