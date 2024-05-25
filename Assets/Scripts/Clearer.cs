using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clearer : MonoBehaviour
{

    public void ClearPieceAt(Cell cell)
    {
        cell.ClearPiece();
    }

    public void ClearPieceAt(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            ClearPieceAt(piece.GetCell());
        }
    }

    public IEnumerator ClearRoutine(Board board, List<Piece> pieces)
    {
        board.HighlightPieces(pieces);
        yield return new WaitForSeconds(0.25f);

        board.GetClearer().ClearPieceAt(pieces);

        yield return new WaitForSeconds(0.25f);
    }
}
