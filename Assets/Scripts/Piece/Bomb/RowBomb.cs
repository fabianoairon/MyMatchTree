using System.Collections.Generic;
using UnityEngine;

public class RowBomb : Piece, IBombPiece
{
    private int _row;

    public List<Piece> Explode()
    {
        List<Piece> explodedPieces = new List<Piece>();
        _row = GetY();

        Board board = GetCell().GetBoard();

        int columns = board.GetWidth();
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < columns; i++)
        {
            explodedPieces.Add(allCells[i, _row].GetPiece());
        }

        return explodedPieces;
    }
}
