using System.Collections.Generic;
using UnityEngine;

public class RowBomb : Piece, IBombPiece
{
    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> explodedPieces = new List<Piece>();
        int row = GetY(); // row = fileira horizontal = cada fileira está num nivel Y

        Board board = GetCell().GetBoard();

        int rowPiecesIndex = board.GetWidth(); // width é quantidade de peças na fileira
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < rowPiecesIndex; i++)
        {
            explodedPieces.Add(allCells[i, row].GetPiece());
        }

        return explodedPieces;
    }
}
