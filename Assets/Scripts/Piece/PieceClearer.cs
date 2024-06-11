using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClearer : MonoBehaviour
{
    public event Action<Piece> OnPieceCleared;

    public void ClearPieceAt(Piece piece)
    {
        piece.GetCell().ClearPiece();
        OnPieceCleared?.Invoke(piece);
    }

    public void ClearPieceAt(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            ClearPieceAt(piece);
        }
    }

    public IEnumerator ClearRoutine(Board board, List<Piece> pieces)
    {
        // se tem bombas em "pieces" adicionar peças que estão no alcance delas em pieces
        board.GetClearer().ClearPieceAt(pieces);
        board.GetCellBreaker().BreakBreakableCells(pieces);

        yield return new WaitForSeconds(0.25f);
    }
}
