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
        public Cell Cell;

        public BombPieceData(PieceSO pieceSO, PieceColor pieceColor, Cell cell)
        {
            BombPieceSO = pieceSO;
            PieceColor = pieceColor;
            Cell = cell;
        }
    }

    private Board _board;

    [SerializeField]
    private PieceSO _areaBomb;
    [SerializeField]
    private PieceSO _rowBomb;
    [SerializeField]
    private PieceSO _columnBomb;
    [SerializeField]
    private PieceSO _colorBomb;

    private List<BombPieceData> _spawnBombList;

    private void Start()
    {
        _board = GetComponent<Board>();
        _board.GetMatcher().OnMatchOccur += PieceMatcher_OnMatchOccur;

        _spawnBombList = new List<BombPieceData>();
    }

    private void PieceMatcher_OnMatchOccur(List<Piece> list)
    {
        GetBombsToSpawn(list);
    }

    public IEnumerator SeekInBombRangePieces(List<Piece> piecesToProcess, Action<List<Piece>> callback)
    {
        List<Piece> piecesPlusBombed = piecesToProcess;

        foreach (var piece in piecesToProcess)
        {
            if (piece is IBombPiece bombPiece)
            {
                piecesPlusBombed = piecesPlusBombed.Union(bombPiece.GetBombedPiecesInRange()).ToList();
            }
        }

        callback(piecesPlusBombed);
        yield return null;
        // new WaitForSeconds(_board.GetCoroutineFinalPauseDuration());
    }

    public void GetBombsToSpawn(List<Piece> piecesToCheckBomb)
    {
        MatchShape matchShape = MatchShape.NONE;
        Piece swapperPiece = piecesToCheckBomb.FirstOrDefault(p => p.IsSwapping());
        PieceSO pieceSOToSpawn = null;

        foreach (var piece in piecesToCheckBomb)
        {
            var horzMatches = _board.GetMatcher().FindHorizontalMatches(_board, piece.GetX(), piece.GetY());
            var vertMatches = _board.GetMatcher().FindVerticalMatches(_board, piece.GetX(), piece.GetY());

            if (horzMatches.Count >= 5 || vertMatches.Count >= 5)
            {
                swapperPiece = swapperPiece == null ? horzMatches.Count > vertMatches.Count ? horzMatches[0] : vertMatches[0] : swapperPiece;
                pieceSOToSpawn = _colorBomb;
                matchShape = MatchShape.FIVE_IN_A_ROW;
                break;
            }
            else if (horzMatches.Count >= 3 && vertMatches.Count >= 3)
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
            _spawnBombList.Add(new BombPieceData(pieceSOToSpawn, swapperPiece.GetPieceColor(), swapperPiece.GetCell()));
        }
    }

    private Piece PlaceBomb(Board board, Cell cell, PieceSO pieceSO)
    {
        Piece piecePlaced = board.GetPiecePlacer().PlacePieceAt(board, cell, pieceSO);
        return piecePlaced;
    }

    public IEnumerator SpawnBombsRoutine(Action<List<Piece>> callback)
    {
        if (_board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("BombManager.SpawnBombsRoutine Started");

        List<Piece> bombsToProcess = new List<Piece>();

        foreach (var bomb in _spawnBombList)
        {
            Piece piece = PlaceBomb(_board, _board.GetCellGrid()[(int)bomb.Cell.GetCoordinate().x, (int)bomb.Cell.GetCoordinate().y], bomb.BombPieceSO);
            piece.SetColorAndSprite(piece, bomb.PieceColor);

            bombsToProcess.Add(piece);
        }

        _spawnBombList.Clear();

        callback(bombsToProcess);

        yield return new WaitForSeconds(_board.GetCoroutineFinalPauseDuration());
        if (_board.GetDebugLogManager().StartAndEndCoroutines) Debug.Log("BombManager.SpawnBombsRoutine Started");
    }
}
