using System;
using System.Collections.Generic;
using UnityEngine;

public class CellBreaker : MonoBehaviour
{
    public event Action<Cell> OnCellBreak;

    public void BreakBreakableCells(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            BreakBreakableCell(piece);
        }
    }

    private void BreakBreakableCell(Piece piece)
    {
        Cell cell = piece.GetCell();
        if (cell.IsBreakable() && cell is BreakableCell breakableCell)
        {
            breakableCell.BreakCell();
            OnCellBreak?.Invoke(cell);
        }
    }
}
