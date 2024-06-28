using UnityEngine;

[CreateAssetMenu(fileName = "StartingCells Asset", menuName = "ScriptableObjects/StartingCells Asset", order = 1)]
public class StartingCellsSO : ScriptableObject
{
    public StartingCell[] StartingCells;
}
