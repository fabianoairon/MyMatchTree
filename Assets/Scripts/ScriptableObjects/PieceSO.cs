using UnityEngine;

[CreateAssetMenu(fileName = "Piece Asset", menuName = "ScriptableObjects/Piece Asset", order = 1)]
public class PieceSO : ScriptableObject
{
    public GameObject _piecePrefab;
    public PieceColor _pieceColor;
    public Texture2D _pieceTextureIcon;
}
