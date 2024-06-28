using UnityEngine;

[CreateAssetMenu(fileName = "StartingPiece Asset", menuName = "ScriptableObjects/StartingPiece Asset", order = 1)]
public class StartingPiecesSO : ScriptableObject
{
    public StartingPiece[] StartingPiecesSet;
}
