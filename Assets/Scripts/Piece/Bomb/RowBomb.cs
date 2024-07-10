using System.Collections.Generic;
using UnityEngine;

public class RowBomb : Piece, IBombPiece
{

    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> explodedPieces = new List<Piece>();
        int row = GetY(); // row = linha horizontal = cada fileira está numa coordenada Y

        Board board = GetCell().GetBoard();

        int rowPiecesIndex = board.GetWidth(); // width é quantidade de peças na fileira
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < rowPiecesIndex; i++)
        {
            Piece piece = allCells[i, row].GetPiece();
            if (piece != null)
            {
                explodedPieces.Add(piece);
            }
        }
        return explodedPieces;
    }
}
