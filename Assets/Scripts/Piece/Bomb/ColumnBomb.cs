using System.Collections.Generic;
using UnityEngine;

public class ColumnBomb : Piece, IBombPiece
{
    private int _column;

    public List<Piece> Explode()
    {
        List<Piece> explodedPieces = new List<Piece>();
        _column = GetX();

        Board board = GetCell().GetBoard();

        int rows = board.GetWidth();
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < rows; i++)
        {
            explodedPieces.Add(allCells[_column, i].GetPiece());
        }

        return explodedPieces;
    }
}
