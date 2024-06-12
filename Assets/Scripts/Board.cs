using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(PieceSwapper))]
[RequireComponent(typeof(PieceClearer))]
[RequireComponent(typeof(PieceCollapser))]
[RequireComponent(typeof(PiecePlacer))]
[RequireComponent(typeof(PieceMatcher))]
[RequireComponent(typeof(CellPlacer))]
[RequireComponent(typeof(CellBreaker))]
[RequireComponent(typeof(BombSeeker))]
public class Board : MonoBehaviour
{
    [Header("Board Dimension")]
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    private Cell[,] _allCells;

    [Header("Scripts References")]

    [SerializeField]
    private CellPlacer _cellPlacer;
    [SerializeField]
    private CellBreaker _cellBreaker;

    [SerializeField]
    private PieceSwapper _swapper;
    [SerializeField]
    private PiecePlacer _piecePlacer;
    [SerializeField]
    private PieceClearer _clearer;
    [SerializeField]
    private PieceCollapser _collapser;
    [SerializeField]
    private PieceMatcher _matcher;

    [SerializeField]
    private BombSeeker _bombSeeker;

    [Header("Durations")]

    [SerializeField]
    private float _swapPieceDuration;
    [SerializeField]
    private float _collapsePieceDuration;
    [SerializeField]
    private float _refillPieceDuration;

    private void Start()
    {
        _allCells = new Cell[_width, _height];
        _cellPlacer.GenerateCellGrid(this);
        _piecePlacer.StartFillBoard(this);
    }

    public List<int> GetColumns(List<Piece> pieces)
    {
        if (pieces == null || pieces.Count == 0) return null;
        List<int> columns = new List<int>();

        foreach (var piece in pieces)
        {
            int column = GetColumn(piece);
            if (!columns.Contains(column))
            {
                columns.Add(column);
            }
        }

        return columns;
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    public float GetMoveDuration(MoveType moveType, Vector3 start, Vector3 end)
    {
        switch (moveType)
        {
            case MoveType.SWAP:
                return _swapPieceDuration;
            case MoveType.COLLAPSE:
                return _collapsePieceDuration * GetYDistanceBetween(start, end);
            case MoveType.REFILL:
                return _refillPieceDuration * GetYDistanceBetween(start, end);
            default:
                return 0.01f;
        }
    }

    public void ClearCollapseRefillMatch(List<Piece> pieces)
    {
        StartCoroutine(ClearCollapseRefillMatchRoutine(pieces));
    }

    public IEnumerator ClearCollapseRefillMatchRoutine(List<Piece> pieces)
    {
        _swapper.SetPlayerCanSwap(false);
        List<Piece> piecesToProcess = pieces;

        do
        {
            //_bombSeeker.SeekBombs(piecesToProcess, callback => UpdatePieceList(callback, ref piecesToProcess));

            yield return StartCoroutine(_clearer.ClearRoutine(this, piecesToProcess));

            yield return StartCoroutine(_collapser.CollpseRoutine(this, piecesToProcess, callback => UpdatePieceList(callback, ref piecesToProcess)));

            yield return StartCoroutine(_piecePlacer.RefillRoutine(this, callback => MergePieceLists(callback, ref piecesToProcess)));

            yield return StartCoroutine(_matcher.MatchRoutine(this, piecesToProcess, callback => UpdatePieceList(callback, ref piecesToProcess)));
        }
        while (piecesToProcess.Count != 0);

        _swapper.SetPlayerCanSwap(true);
    }

    private void UpdatePieceList(List<Piece> sourceList, ref List<Piece> targetList)
    {
        targetList = sourceList;
    }

    private void MergePieceLists(List<Piece> sourceList, ref List<Piece> targetList)
    {
        targetList = targetList.Union(sourceList).ToList();
    }

    public int GetColumn(Piece piece)
    {
        return piece.GetX();
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public Cell[,] GetCellGrid()
    {
        return _allCells;
    }

    private int GetYDistanceBetween(Vector3 start, Vector3 end)
    {
        return Mathf.Abs((int)start.y - (int)end.y);
    }

    public PiecePlacer GetPiecePlacer()
    {
        return _piecePlacer;
    }

    public PieceClearer GetClearer()
    {
        return _clearer;
    }

    public PieceCollapser GetCollapser()
    {
        return _collapser;
    }

    public PieceSwapper GetSwapper()
    {
        return _swapper;
    }

    public PieceMatcher GetMatcher()
    {
        return _matcher;
    }

    public CellPlacer GetCellPlacer()
    {
        return _cellPlacer;
    }

    public CellBreaker GetCellBreaker()
    {
        return _cellBreaker;
    }

    public BombSeeker GetBombSeeker()
    {
        return _bombSeeker;
    }

    private void HightlightPiece(Piece piece)
    {
        if (piece == null) return;
        SpriteRenderer cellRenderer = piece.GetCell().GetComponent<SpriteRenderer>();
        SpriteRenderer pieceRenderer = piece.GetComponent<SpriteRenderer>();

        cellRenderer.color = pieceRenderer.color;
    }

    public void HightlightPieceOff(Piece piece)
    {
        Cell cell = piece.GetCell();

        if (cell.GetCellType() == CellType.NORMAL)
        {
            SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.color = _cellPlacer.GetNormalCellColor();
        }
    }

    public void HighlightPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            HightlightPiece(piece);
        }
    }
}
