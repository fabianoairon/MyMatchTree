using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [Header("Script References")]

    [SerializeField]
    private CellBreaker _cellBreaker;
    [SerializeField]
    private PieceClearer _pieceClearer;

    [Header("Prefab References")]
    [SerializeField]
    private GameObject _pieceClearFXPrefab;
    [SerializeField]
    private GameObject _cellBreakFXPrefab;

    private void Start()
    {
        _cellBreaker.OnCellBreak += CellBreaker_OnCellBreaker;
        _pieceClearer.OnPieceCleared += PieceClearer_OnPieceCleared;
    }

    private void PieceClearer_OnPieceCleared(Piece piece)
    {
        GameObject pieceClearFX = Instantiate(_pieceClearFXPrefab, new Vector3(piece.GetX(), piece.GetY(), 0), Quaternion.identity);
        pieceClearFX.GetComponent<Particle>().SetPieceSpriteColor(piece.GetPieceSpriteColor());
    }

    private void CellBreaker_OnCellBreaker(Cell cell)
    {
        GameObject cellBreakFX = Instantiate(_cellBreakFXPrefab, new Vector3(cell.GetCoordinate().x, cell.GetCoordinate().y, 0), Quaternion.identity);
    }
}