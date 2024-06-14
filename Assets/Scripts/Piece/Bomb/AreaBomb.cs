using System.Collections.Generic;
using UnityEngine;

public class AreaBomb : Piece, IBombPiece
{
    private int _areaOffset = 1;

    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> explodedPieces = new List<Piece>();

        Board board = GetCell().GetBoard();

        Cell[,] allCells = board.GetCellGrid();

        for (int i = GetX() - _areaOffset; i <= GetX() + _areaOffset; i++)
        {
            for (int j = GetY() - _areaOffset; j <= GetY() + _areaOffset; j++)
            {
                if (GetCell().GetBoard().IsWithinBounds(i, j))
                {
                    explodedPieces.Add(allCells[i, j].GetPiece());
                }
            }
        }

        return explodedPieces;
    }
}
