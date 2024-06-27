using UnityEngine;

[CreateAssetMenu(fileName = "Board Asset", menuName = "ScriptableObjects/Board Asset", order = 1)]
public class StartingBoardSO : ScriptableObject
{
    public StartingCell[] StartingCells;
    public StartingPiece[] StartingPiecesSet;
}
