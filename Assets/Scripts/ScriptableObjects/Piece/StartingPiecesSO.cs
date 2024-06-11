using UnityEngine;

[System.Serializable]
public class StartingPiece
{
    public PieceSO PieceSO;
    public int X;
    public int Y;
}

[CreateAssetMenu(fileName = "StartingPiece Asset", menuName = "ScriptableObjects/StartingPiece Asset", order = 1)]
public class StartingPiecesSO : ScriptableObject
{
    public StartingPiece[] StartingPieces;
}
