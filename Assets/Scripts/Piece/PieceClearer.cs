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
        Debug.Log("PieceCleaner.ClearRoutine Started");
        board.GetClearer().ClearPieceAt(pieces);
        board.GetCellBreaker().BreakBreakableCells(pieces);

        yield return new WaitForSeconds(board.GetCoroutineFinalPauseDuration());
        Debug.Log("PieceCleaner.ClearRoutine Ended");
    }
}
