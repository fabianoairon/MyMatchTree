using UnityEngine;
using System.Linq;

public class CellPlacer : MonoBehaviour
{

    [SerializeField]
    private CellSO[] _cellSO;

    public void GenerateCellGrid(Board board)
    {
        for (int x = 0; x < board.GetWidth(); x++)
        {
            for (int y = 0; y < board.GetHeight(); y++)
            {
                PlaceGrid(board, x, y, CellType.NORMAL);
            }
        }
    }

    private void PlaceGrid(Board board, int x, int y, CellType cellType)
    {
        CellSO cellSO = _cellSO.First(cell => cell._cellType == cellType);

        GameObject instantiatedCell = Instantiate(cellSO._cellPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        Cell cell = instantiatedCell.GetComponent<Cell>();

        board.GetCellGrid()[x, y] = cell;
        cell.Setup(x, y, board, cellType);

        instantiatedCell.transform.parent = transform;
    }
}
