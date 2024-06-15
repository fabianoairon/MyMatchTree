using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceCollapser : MonoBehaviour
{
    private List<Piece> CollapseColumn(Board board, int column)
    {
        List<Piece> movingPieces = new List<Piece>();
        Cell[,] cellGrid = board.GetCellGrid();

        for (int i = 0; i < board.GetHeight() - 1; i++)
        {
            if (cellGrid[column, i].GetPiece() == null && cellGrid[column, i].GetCellType() != CellType.OBSTACLE)
            {
                for (int j = i + 1; j < board.GetHeight(); j++)
                {
                    if (cellGrid[column, j].GetPiece() != null)
                    {
                        Piece piece = cellGrid[column, j].GetPiece();
                        cellGrid[column, j].SetPiece(null);

                        piece.Move(cellGrid[column, i].GetCoordinate(), MoveType.COLLAPSE);
                        cellGrid[column, i].SetPiece(piece);
                        piece.SetCell(cellGrid[column, i]);

                        if (!movingPieces.Contains(piece))
                        {
                            movingPieces.Add(piece);
                            break;
                        }
                    }
                }
            }
        }
        return movingPieces;
    }

    public List<Piece> CollapseColumns(Board board, List<Piece> pieces)
    {
        List<int> columns = board.GetColumns(pieces);
        List<Piece> movingPieces = new List<Piece>();

        if (columns == null || columns.Count == 0) return new List<Piece>();

        foreach (var column in columns)
        {
            movingPieces = movingPieces.Union(CollapseColumn(board, column)).ToList();
        }
        return movingPieces;
    }

    public bool IsCollapseEnded(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            if (piece.IsMoving())
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerator CollpseRoutine(Board board, List<Piece> pieces, Action<List<Piece>> callback)
    {
        Debug.Log("PieceCollapser.CollapseRoutine Started");

        var movingPieces = board.GetCollapser().CollapseColumns(board, pieces);

        while (!IsCollapseEnded(movingPieces))
        {
            yield return null;
        }

        callback(movingPieces);

        yield return new WaitForSeconds(.5f);
        Debug.Log("PieceCollapser.CollapseRoutine Ended");
    }


}
