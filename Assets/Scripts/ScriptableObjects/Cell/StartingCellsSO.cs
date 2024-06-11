using UnityEngine;

[System.Serializable]
public class StartingCell
{
    public CellSO CellSO;
    public int X;
    public int Y;
    public int InitialBreakablePhase;
}

[CreateAssetMenu(fileName = "StartingCells Asset", menuName = "ScriptableObjects/StartingCells Asset", order = 1)]
public class StartingCellsSO : ScriptableObject
{
    public StartingCell[] StartingCells;
}
