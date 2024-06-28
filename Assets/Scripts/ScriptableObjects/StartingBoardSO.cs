using UnityEngine;

[System.Serializable]
public class StartingCell
{
    public CellSO CellSO;
    public int X;
    public int Y;
    public int InitialBreakablePhase;
}

[System.Serializable]
public class StartingPiece
{
    public PieceSO PieceSO;
    public int X;
    public int Y;
}

[CreateAssetMenu(fileName = "Board Asset", menuName = "ScriptableObjects/Board Asset", order = 1)]
public class StartingBoardSO : ScriptableObject
{
    public int boardWidth;
    public int boardHeight;

    public StartingCell[] StartingCellsSet;
    public StartingPiece[] StartingPiecesSet;
}
