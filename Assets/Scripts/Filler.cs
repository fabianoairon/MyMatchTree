using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filler : MonoBehaviour
{
    public List<Piece> FillWithRandomPiece(Board board, int yOffset = 0)
    {
        List<Piece> pieces = new List<Piece>();

        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                if (board.GetCellGrid()[x, y].GetPiece() == null)
                {
                    Cell cell = board.GetCellGrid()[x, y];
                    Piece piece = board.GetPiecePlacer().PlaceRandomPieceAt(board, cell);


                    if (yOffset == 0)
                    {
                        while (board.GetMatcher().HasMatchesAt(board, x, y))
                        {
                            board.GetPiecePlacer().SwitchPieceWithAnotherRandom(board, cell);
                        }
                    }
                    else
                    {
                        piece.SetPosition(piece.GetX(), piece.GetY(), 0, yOffset);
                        piece.Move(piece.GetCell().GetCoordinate(), MoveType.REFILL);
                    }

                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

    public IEnumerator RefillRoutine(Board board, Action<List<Piece>> callback)
    {
        var filledPieces = FillWithRandomPiece(board, 7);

        while (!board.GetCollapser().IsCollapseEnded(filledPieces))
        {
            yield return null;
        }

        callback(filledPieces);
    }
}
