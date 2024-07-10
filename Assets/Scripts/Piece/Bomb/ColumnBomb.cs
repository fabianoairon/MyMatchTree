using System.Collections.Generic;

public class ColumnBomb : Piece, IBombPiece
{

    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> explodedPieces = new List<Piece>();
        int column = GetX(); // column = linha vertical = cada coluna está numa coordenada X

        Board board = GetCell().GetBoard();

        int columnPiecesIndex = board.GetHeight(); // height é quantidade de peças na coluna
        Cell[,] allCells = board.GetCellGrid();

        for (int i = 0; i < columnPiecesIndex; i++)
        {
            Piece piece = allCells[column, i].GetPiece();
            if (piece != null)
            {
                explodedPieces.Add(piece);
            }
        }
        return explodedPieces;
    }
}
