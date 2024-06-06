using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CellPlacer : MonoBehaviour
{

    [SerializeField]
    private CellSO[] _cellSO;

    [SerializeField]
    private StartingCell[] _startingCells;

    [SerializeField]
    private Sprite[] _breakableSprites;

    [SerializeField]
    private Color _normalCellColor = new Color(1f, 1f, 1f, 0.017f);
    [SerializeField]
    private Color _breakableCellColor = new Color(1f, 1f, 1f, 0.017f);



    public void GenerateCellGrid(Board board)
    {
        if (_startingCells.Length > 0)
        {
            StartingCellsGenerator(board, _startingCells);
        }


        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                if (board.GetCellGrid()[x, y] == null)
                {
                    PlaceCell(board, x, y, CellType.NORMAL);
                }
            }
        }
    }

    public Cell PlaceCell(Board board, int x, int y, CellType cellType, int initialValue = 0)
    {
        CellSO cellSO = _cellSO.First(cell => cell._cellType == cellType);

        GameObject instantiatedCell = Instantiate(cellSO._cellPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        Cell cell = instantiatedCell.GetComponent<Cell>();

        board.GetCellGrid()[x, y] = cell;
        cell.Setup(x, y, board, cellType, initialValue);

        instantiatedCell.transform.parent = transform;
        return cell;
    }

    public Cell PlaceCell(Board board, StartingCell startingCell)
    {
        return PlaceCell(board, startingCell._x, startingCell._y, startingCell._cellType, startingCell._initialBreakablePhase);
    }

    private void StartingCellsGenerator(Board board, StartingCell[] startingCells)
    {
        foreach (var singleCell in startingCells)
        {
            if (board.GetCellGrid()[singleCell._x, singleCell._y] == null)
            {
                PlaceCell(board, singleCell);
            }
        }
    }

    public void ReplaceWithNewNormalCell(Board board, Cell oldCell)
    {
        int x = (int)oldCell.GetCoordinate().x;
        int y = (int)oldCell.GetCoordinate().y;

        Cell newCell = PlaceCell(board, x, y, CellType.NORMAL);

        board.GetCellGrid()[x, y] = newCell;

        Destroy(oldCell.gameObject);
    }

    public Sprite GetCurrentBreakableSprite(int phase)
    {
        return _breakableSprites[--phase];
    }

    public void BreakBreakableCells(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            Cell cell = piece.GetCell();
            if (cell.GetCellType() == CellType.BREAKABLE)
            {
                ((BreakableCell)cell).BreakCell();
            }
        }
    }

    public Color GetNormalCellColor()
    {
        return _normalCellColor;
    }

    public Color GetBreakableCellColor()
    {
        return _breakableCellColor;
    }
}
