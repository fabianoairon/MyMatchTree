using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AllPiecesAndCellsSO")]
public class AllPiecesAndCellsSO : ScriptableObject
{
	public CellSO[] cells;
	public PieceSO[] pieces;
}