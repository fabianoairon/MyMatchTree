using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombSeeker : MonoBehaviour
{
    public void SeekBombs(List<Piece> piecesToProcess, Action<List<Piece>> callback)
    {
        List<Piece> bombPieces = new List<Piece>();

        foreach (var piece in piecesToProcess)
        {
            if (piece is IBombPiece bombPiece)
            {
                bombPieces.Union(bombPiece.Explode()).ToList();
            }
        }

        callback(bombPieces);
    }
}
