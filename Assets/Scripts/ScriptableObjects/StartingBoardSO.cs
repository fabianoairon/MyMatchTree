using UnityEngine;

[System.Serializable]
public class StartingCell
{
    public CellSO CellSO;
    public int X;
    public int Y;

    public StartingCell(CellSO cell, int x, int y)
    {
        CellSO = cell;
        X = x;
        Y = y;
    }
}

[System.Serializable]
public class StartingPiece
{
    public PieceSO PieceSO;
    public int X;
    public int Y;

    public StartingPiece(PieceSO piece, int x, int y)
    {
        PieceSO = piece;
        X = x;
        Y = y;
    }
}

[CreateAssetMenu(fileName = "Board Asset", menuName = "ScriptableObjects/Board Asset", order = 1)]
public class StartingBoardSO : ScriptableObject
{
    public int boardWidth;
    public int boardHeight;

    public AllPiecesAndCellsSO AllPiecesAndCells;
    public StartingCell[] StartingCellsSet;
    public StartingPiece[] StartingPiecesSet;
}
