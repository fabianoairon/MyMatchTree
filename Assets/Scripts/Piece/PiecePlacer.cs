using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

public class PiecePlacer : MonoBehaviour
{

    [SerializeField]
    private PieceSO[] _piecesSO;

    public void StartFillBoard(Board board, int yOffset = 0)
    {
        var sPieces = board.GetStartingPieces();

        if (sPieces != null && sPieces.Count() > 0)
        {
            foreach (var sPiece in sPieces)
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
                    Piece piece = board.GetPiecePlacer().PlacePieceAt(board, cell);

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
        if (board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("PiecePlacer.RefillRoutine Started");
        var filledPieces = FillWithRandomPiece(board, 7);

        while (!board.GetCollapser().IsCollapseEnded(filledPieces))
        {
            yield return null;
        }

        callback(filledPieces);
        yield return new WaitForSeconds(board.GetCoroutineFinalPauseDuration());
        if (board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("PiecePlacer.RefillRoutine Ended");
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

    public Piece PlacePieceAt(Board board, Cell cell, PieceSO pieceSO = null)
    {
        if (cell.GetCellType() == CellType.OBSTACLE) return null;

        GameObject gameObject;

        if (pieceSO == null)
        {
            gameObject = GetRandomPieceGameObject(board);
        }
        else
        {
            gameObject = GetGOPieceByPieceSO(pieceSO);
        }

        Piece piece = GetPieceFromGOPiece(gameObject);
        PlacePieceAt(board, cell, piece);
        return piece;
    }

    public void SwitchPieceWithAnotherRandom(Board board, Cell cell)
    {
        cell.ClearPiece();
        PlacePieceAt(board, cell);
    }

    private GameObject GetRandomPieceGameObject(Board board)
    {
        int randomIndex = UnityEngine.Random.Range(0, _piecesSO.Length);
        return GetGOPieceByPieceSO(_piecesSO[randomIndex]);
    }

    private GameObject GetGOPieceByPieceSO(PieceSO pieceSO)
    {
        Piece piece = GetPieceFromGOPiece(Instantiate(pieceSO._piecePrefab));

        piece.SetPieceColor(pieceSO._pieceColor);

        if (!(piece is ColorBomb))
        {
            piece.GetComponent<SpriteRenderer>().color = pieceSO._pieceColor.ToColor();
        }
        return piece.gameObject;
    }

    private void PlaceStartingPieceAt(Board board, StartingPiece startingPiece)
    {
        Cell cell = board.GetCellGrid()[startingPiece.X, startingPiece.Y];

        Piece piece = GetPieceFromGOPiece(GetGOPieceByPieceSO(startingPiece.PieceSO));
        PlacePieceAt(board, cell, piece);
    }

    private Piece GetPieceFromGOPiece(GameObject gameObject)
    {
        return gameObject.GetComponent<Piece>();
    }
}
