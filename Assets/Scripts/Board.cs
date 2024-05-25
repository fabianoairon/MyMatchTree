using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Swapper))]
[RequireComponent(typeof(Clearer))]
[RequireComponent(typeof(Collapser))]
[RequireComponent(typeof(PiecePlacer))]
public class Board : MonoBehaviour
{
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    [SerializeField]
    private GameObject _cellPrefab;

    private Cell[,] _allCells;

    [SerializeField]
    private Swapper _swapper;
    [SerializeField]
    private PiecePlacer _piecePlacer;
    [SerializeField]
    private Clearer _clearer;
    [SerializeField]
    private Collapser _collapser;
    [SerializeField]
    private Matcher _matcher;
    [SerializeField]
    private Filler _filler;


    [SerializeField]
    private float _swapPieceDuration;
    [SerializeField]
    private float _collapsePieceDuration;
    [SerializeField]
    private float _refillPieceDuration;

    private void Start()
    {
        _allCells = new Cell[_width, _height];
        GenerateCellGrid();
        _filler.FillWithRandomPiece(this);
    }

    private void GenerateCellGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject instantiatedCell = Instantiate(_cellPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                Cell cell = instantiatedCell.GetComponent<Cell>();
                _allCells[x, y] = cell;
                cell.Setup(x, y, this);
                instantiatedCell.transform.parent = transform;
            }
        }
    }



    public int GetColumn(Piece piece)
    {
        return piece.GetX();
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



    private int GetYDistanceBetween(Vector3 start, Vector3 end)
    {
        return Mathf.Abs((int)start.y - (int)end.y);
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

    public PiecePlacer GetPiecePlacer()
    {
        return _piecePlacer;
    }

    public Clearer GetClearer()
    {
        return _clearer;
    }

    public Collapser GetCollapser()
    {
        return _collapser;
    }

    public Swapper GetSwapper()
    {
        return _swapper;
    }

    public Matcher GetMatcher()
    {
        return _matcher;
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
            yield return StartCoroutine(_clearer.ClearRoutine(this, piecesToProcess));

            yield return StartCoroutine(_collapser.CollpseRoutine(this, piecesToProcess, callback => UpdatePieceList(callback, ref piecesToProcess)));

            yield return StartCoroutine(_filler.RefillRoutine(this, callback => MergePieceLists(callback, ref piecesToProcess)));

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

    private void HightlightPiece(Piece piece)
    {
        if (piece == null) return;
        SpriteRenderer cellRenderer = piece.GetCell().GetComponent<SpriteRenderer>();
        SpriteRenderer pieceRenderer = piece.GetComponent<SpriteRenderer>();

        cellRenderer.color = pieceRenderer.color;
    }

    public void HightlightPieceOff(Piece piece)
    {
        SpriteRenderer cellRenderer = piece.GetCell().GetComponent<SpriteRenderer>();
        cellRenderer.color = new Color(0f, 0f, 0f, 0f);
    }

    public void HighlightPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            HightlightPiece(piece);
        }
    }
}
