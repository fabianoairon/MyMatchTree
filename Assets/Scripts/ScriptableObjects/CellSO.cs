using UnityEngine;

[CreateAssetMenu(fileName = "Cell Asset", menuName = "ScriptableObjects/Cell Asset", order = 1)]
public class CellSO : ScriptableObject
{
    public GameObject _cellPrefab;
    public CellType _cellType;
}
