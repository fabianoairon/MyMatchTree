using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CellPlacer : MonoBehaviour
{
    [Header("Starting Cells")]
    [SerializeField]
    private StartingCellsSO _startingCellsSO;

    [Header("All Possible Cells")]
    [SerializeField]
    private CellSO[] _cellSO;

    [Header("Other")]
    [SerializeField]
    private Sprite[] _breakableSprites;

    [SerializeField]
    private Color _normalCellColor = new Color(1f, 1f, 1f, 0.017f);
    [SerializeField]
    private Color _breakableCellColor = new Color(1f, 1f, 1f, 0.017f);



    public void GenerateCellGrid(Board board)
    {
        if (_startingCellsSO != null)
        {
            StartingCellsGenerator(board, _startingCellsSO);
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

    public Cell PlaceCell(Board board, StartingCell sCell)
    {
        return PlaceCell(board, sCell.X, sCell.Y, sCell.CellSO._cellType, sCell.InitialBreakablePhase);
    }

    private void StartingCellsGenerator(Board board, StartingCellsSO sCellsSO)
    {
        foreach (var cell in sCellsSO.StartingCells)
        {
            if (board.IsWithinBounds(cell.X, cell.Y) && board.GetCellGrid()[cell.X, cell.Y] == null)
            {
                PlaceCell(board, cell);
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

    public Color GetNormalCellColor()
    {
        return _normalCellColor;
    }

    public Color GetBreakableCellColor()
    {
        return _breakableCellColor;
    }
}
