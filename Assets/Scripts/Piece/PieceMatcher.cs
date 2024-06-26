using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceMatcher : MonoBehaviour
{
    public event Action<List<Piece>> OnMatchOccur;

    private List<Piece> FindMatches(Board board, int startX, int startY, Vector2 direction, int minCount = 3)
    {
        List<Piece> matchPieces = new List<Piece>();
        Cell[,] allCells = board.GetCellGrid();

        Piece piece = allCells[startX, startY].GetPiece();

        if (piece != null)
        {
            matchPieces.Add(piece);
        }

        int maxLenght = board.GetWidth() > board.GetHeight() ? board.GetWidth() : board.GetHeight();

        for (int i = 1; i < maxLenght; i++)
        {
            int nextX = startX + (int)direction.x * i;
            int nextY = startY + (int)direction.y * i;

            if (!board.IsWithinBounds(nextX, nextY))
            {
                break;
            }

            Piece nextPiece = allCells[nextX, nextY].GetPiece();

            if (nextPiece != null && piece.GetPieceColor() == nextPiece.GetPieceColor())
            {
                matchPieces.Add(nextPiece);
            }
            else
            {
                break;
            }
        }

        return matchPieces.Count >= minCount ? matchPieces : new List<Piece>();
    }

    public List<Piece> FindHorizontalMatches(Board board, int startX, int startY, int minCount = 3)
    {
        List<Piece> leftMatches = FindMatches(board, startX, startY, Vector2.left, 2);
        List<Piece> rightMatches = FindMatches(board, startX, startY, Vector2.right, 2);

        var horizontalMatches = leftMatches.Union(rightMatches).ToList();

        return horizontalMatches.Count >= minCount ? horizontalMatches : new List<Piece>();
    }

    public List<Piece> FindVerticalMatches(Board board, int startX, int startY, int minCount = 3)
    {
        List<Piece> upMatches = FindMatches(board, startX, startY, Vector2.up, 2);
        List<Piece> downMatches = FindMatches(board, startX, startY, Vector2.down, 2);

        var verticalMatches = upMatches.Union(downMatches).ToList();

        return verticalMatches.Count >= minCount ? verticalMatches : new List<Piece>();
    }

    public List<Piece> FindAllMatchesAtXAndY(Board board, int startX, int startY, int minCount = 3)
    {
        List<Piece> horizontalMatches = FindHorizontalMatches(board, startX, startY, minCount);
        List<Piece> verticalMatches = FindVerticalMatches(board, startX, startY, minCount);

        var allMatches = horizontalMatches.Union(verticalMatches).ToList();

        return allMatches.Count >= minCount ? allMatches : new List<Piece>();
    }

    public List<Piece> FindAllMatchesByPiece(Board board, Piece piece, int minCount = 3)
    {
        return FindAllMatchesAtXAndY(board, piece.GetX(), piece.GetY(), minCount);
    }

    public List<Piece> FindAllMatchesOnListOfPieces(Board board, List<Piece> pieces, int minCount = 3)
    {
        List<Piece> matches = new List<Piece>();
        foreach (var piece in pieces)
        {
            matches = matches.Union(FindAllMatchesByPiece(board, piece, minCount)).ToList();
        }

        return matches;
    }

    public List<Piece> FindAllMatchesTwoPieces(Board board, Piece pieceOne, Piece pieceTwo, int minCount = 3)
    {
        var matchesOne = FindAllMatchesByPiece(board, pieceOne, minCount);
        var matchesTwo = FindAllMatchesByPiece(board, pieceTwo, minCount);

        var allMatches = matchesOne.Union(matchesTwo).ToList();

        OnMatchOccur?.Invoke(matchesOne);
        OnMatchOccur?.Invoke(matchesTwo);

        return allMatches;
    }

    public bool HasMatchesAt(Board board, int x, int y, int minCount = 3)
    {
        List<Piece> leftPieces = FindMatches(board, x, y, Vector2.left, minCount);
        List<Piece> downPieces = FindMatches(board, x, y, Vector2.down, minCount);
        List<Piece> rightPieces = FindMatches(board, x, y, Vector2.right, minCount);
        List<Piece> upPieces = FindMatches(board, x, y, Vector2.up, minCount);

        if (leftPieces.Count >= minCount ||
        downPieces.Count >= minCount ||
        rightPieces.Count >= minCount ||
        upPieces.Count >= minCount)
        {
            return true;
        }
        return false;
    }

    public IEnumerator MatchRoutine(Board board, List<Piece> pieces, Action<List<Piece>> callback)
    {
        if (board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("PieceMatcher.MatchRoutine Started");
        var matches = FindAllMatchesOnListOfPieces(board, pieces);
        OnMatchOccur?.Invoke(matches);
        callback(matches);
        yield return new WaitForSeconds(board.GetCoroutineFinalPauseDuration());
        if (board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("PieceMatcher.MatchRoutine Ended");
    }

}
