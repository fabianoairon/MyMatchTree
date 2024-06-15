using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public struct BombPieceData
    {
        public PieceSO BombPieceSO;
        public PieceColor PieceColor;
        public Color Color;
        public Cell Cell;

        public BombPieceData(PieceSO pieceSO, PieceColor pieceColor, Color color, Cell cell)
        {
            BombPieceSO = pieceSO;
            PieceColor = pieceColor;
            Color = color;
            Cell = cell;
        }
    }

    Board board;

    [SerializeField]
    private PieceSO _areaBomb;
    [SerializeField]
    private PieceSO _rowBomb;
    [SerializeField]
    private PieceSO _columnBomb;

    private List<BombPieceData> _spawnBombList;

    private void Start()
    {
        board = GetComponent<Board>();
        board.GetMatcher().OnMatchOccur += PieceMatcher_OnMatchOccur;

        _spawnBombList = new List<BombPieceData>();
    }

    private void PieceMatcher_OnMatchOccur(List<Piece> list)
    {
        GetBombsToSpawn(list);
    }

    public IEnumerator PutBombsInRangeToClear(List<Piece> piecesToProcess, Action<List<Piece>> callback)
    {
        List<Piece> bombPieces = piecesToProcess;

        foreach (var piece in piecesToProcess)
        {
            if (piece is IBombPiece bombPiece)
            {
                bombPieces = bombPieces.Union(bombPiece.GetBombedPiecesInRange()).ToList();
            }
        }

        callback(bombPieces);
        yield return new WaitForSeconds(0.5f);
    }

    public void GetBombsToSpawn(List<Piece> piecesToCheckBomb)
    {
        MatchShape matchShape = MatchShape.NONE;
        Piece swapperPiece = piecesToCheckBomb.FirstOrDefault(p => p.IsSwapping());
        PieceSO pieceSOToSpawn = null;

        foreach (var piece in piecesToCheckBomb)
        {
            var horzMatches = board.GetMatcher().FindHorizontalMatches(board, piece.GetX(), piece.GetY());
            var vertMatches = board.GetMatcher().FindVerticalMatches(board, piece.GetX(), piece.GetY());

            if (horzMatches.Count >= 3 && vertMatches.Count >= 3)
            {
                swapperPiece = swapperPiece == null ? horzMatches.First(p => vertMatches.Contains(p)) : swapperPiece;
                pieceSOToSpawn = _areaBomb;
                matchShape = MatchShape.THREE_90_DEGREES;
                break;
            }
            else if (horzMatches.Count > 3 && vertMatches.Count == 0)
            {
                swapperPiece = swapperPiece == null ? horzMatches[0] : swapperPiece;
                pieceSOToSpawn = _columnBomb;
                matchShape = MatchShape.FOUR_IN_A_ROW;
                break;
            }
            else if (horzMatches.Count == 0 && vertMatches.Count > 3)
            {
                swapperPiece = swapperPiece == null ? vertMatches[0] : swapperPiece;
                pieceSOToSpawn = _rowBomb;
                matchShape = MatchShape.FOUR_IN_A_COLUMN;
                break;
            }
        }

        if (matchShape != MatchShape.NONE && pieceSOToSpawn != null)
        {
            _spawnBombList.Add(new BombPieceData(pieceSOToSpawn, swapperPiece.GetPieceColor(), swapperPiece.GetPieceSpriteColor(), swapperPiece.GetCell()));
        }
    }

    private Piece PlaceBomb(Board board, Cell cell, PieceSO pieceSO)
    {
        Piece piecePlaced = board.GetPiecePlacer().PlacePieceAt(board, cell, pieceSO);
        return piecePlaced;
    }



    private void SpawnBombs()
    {
        foreach (var bomb in _spawnBombList)
        {
            Piece piece = PlaceBomb(board, bomb.Cell, bomb.BombPieceSO);
            piece.SetColorAndSprite(piece, bomb.PieceColor, bomb.Color);
        }

        _spawnBombList.Clear();
    }

    public IEnumerator SpawnBombsRoutine()
    {
        Debug.Log("BombManager.SpawnBombsRoutine Started");
        SpawnBombs();
        yield return new WaitForSeconds(.5f);
        Debug.Log("BombManager.SpawnBombsRoutine Started");
    }

}
