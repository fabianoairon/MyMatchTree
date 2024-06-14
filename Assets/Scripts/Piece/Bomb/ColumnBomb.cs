using System.Collections.Generic;

public class ColumnBomb : Piece, IBombPiece
{
    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> explodedPieces = new List<Piece>();
        int column = GetX(); // row = coluna = cada colluna está num nivel X

        Board board = GetCell().GetBoard();

        int columnPiecesIndex = board.GetHeight(); // heighg é quantidade de peças na coluna
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < columnPiecesIndex; i++)
        {
            explodedPieces.Add(allCells[column, i].GetPiece());
        }

        return explodedPieces;
    }
}
