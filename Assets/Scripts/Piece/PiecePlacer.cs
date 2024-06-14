using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

public class PiecePlacer : MonoBehaviour
{
    [SerializeField]
    private StartingPiecesSO _startingPiecesSO;

    [SerializeField]
    private PieceSO[] _piecesSO;

    public void StartFillBoard(Board board, int yOffset = 0)
    {
        if (_startingPiecesSO != null && _startingPiecesSO.StartingPiecesSet.Count() > 0)
        {
            foreach (var sPiece in _startingPiecesSO.StartingPiecesSet)
            {
                PlaceStartingPieceAt(board, sPiece);
            }
        }

        FillWithRandomPiece(board);
    }

    public List<Piece> FillWithRandomPiece(Board board, int yOffset = 0)
    {
        List<Piece> pieces = new List<Piece>();

        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                Cell cell = board.GetCellGrid()[x, y];

                if (board.GetCellGrid()[x, y].GetPiece() == null && cell.GetCellType() != CellType.OBSTACLE)
                {
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

    public void PlacePieceAt(Board board, Cell cell, Piece piece)
    {
        piece.transform.position = cell.GetCoordinate();
        piece.transform.rotation = Quaternion.identity;
        piece.transform.parent = transform;
        piece.gameObject.name = "Piece: " + cell.GetCoordinate().x + "." + cell.GetCoordinate().y + " - " + piece.GetPieceColor();

        if (board.IsWithinBounds((int)cell.GetCoordinate().x, (int)cell.GetCoordinate().y))
        {
            cell.SetPiece(piece);
            //board.HightlightPieceOff(piece);
        }
    }

    public void PlacePieceAt(Board board, int x, int y, Piece piece)
    {
        PlacePieceAt(board, board.GetCellGrid()[x, y], piece);
    }

    public Piece PlaceRandomPieceAt(Board board, Cell cell)
    {
        if (cell.GetCellType() == CellType.OBSTACLE) return null;

        GameObject randomPieceGO = GetRandomPieceGameObject();
        Piece piece = randomPieceGO.GetComponent<Piece>();
        PlacePieceAt(board, cell, piece);
        return piece;
    }

    public void SwitchPieceWithAnotherRandom(Board board, Cell cell)
    {
        cell.ClearPiece();
        PlaceRandomPieceAt(board, cell);
    }

    private GameObject GetRandomPieceGameObject()
    {
        int randomIndex = UnityEngine.Random.Range(0, _piecesSO.Length);
        return GetPieceGameObjectByPieceSO(_piecesSO[randomIndex]);
    }

    private GameObject GetPieceGameObjectByPieceSO(PieceSO pieceSO)
    {
        GameObject pieceGO = Instantiate(pieceSO._piecePrefab);
        Piece piece = pieceGO.GetComponent<Piece>();
        piece.SetPieceColor(pieceSO._pieceColor);
        return pieceGO;
    }

    private void PlaceStartingPieceAt(Board board, StartingPiece startingPiece)
    {
        Cell cell = board.GetCellGrid()[startingPiece.X, startingPiece.Y];

        GameObject PieceGO = GetPieceGameObjectByPieceSO(startingPiece.PieceSO);
        Piece piece = PieceGO.GetComponent<Piece>();
        PlacePieceAt(board, cell, piece);
    }
}
