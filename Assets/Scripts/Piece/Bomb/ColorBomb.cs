using System.Collections.Generic;
using System.Linq;

public class ColorBomb : Piece, IBombPiece
{
    public List<Piece> GetBombedPiecesInRange()
    {
        List<Piece> bombedPieces = new List<Piece>();

        if (GetPieceColor() != PieceColor.WILD)
        {
            Board board = GetCell().GetBoard();
            bombedPieces = bombedPieces.Union(board.GetMatcher().FindAllPiecesByColor(board, GetPieceColor())).ToList();
        }

        return bombedPieces;
    }
}
