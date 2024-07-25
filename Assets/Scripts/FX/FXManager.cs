using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [Header("Script References")]

    [SerializeField]
    private Board _board;
    [SerializeField]
    private CellBreaker _cellBreaker;
    [SerializeField]
    private PieceClearer _pieceClearer;

    [Header("Prefab References")]
    [SerializeField]
    private GameObject _pieceClearFXPrefab;
    [SerializeField]
    private GameObject _cellBreakFXPrefab;
    [SerializeField]
    private GameObject _areaBombFXPrefab;
    [SerializeField]
    private GameObject _lineBombFXPrefab;

    [Header("Values")]
    [SerializeField]
    private float _lineBombSpeed;

    private void Start()
    {
        _cellBreaker.OnCellBreak += CellBreaker_OnCellBreaker;
        _pieceClearer.OnPieceCleared += PieceClearer_OnPieceCleared;
    }

    private void PieceClearer_OnPieceCleared(Piece piece)
    {
        switch (piece.GetPieceType())
        {
            case PieceType.AREA_BOMB:
                InstantiateFX(piece, _areaBombFXPrefab);
                break;
            case PieceType.COLUMN_BOMB:
                GameObject downFX = InstantiateFX(piece, _lineBombFXPrefab);
                GameObject upFX = InstantiateFX(piece, _lineBombFXPrefab);

                SetSpeedAndDirection(upFX, Vector3.up);
                SetSpeedAndDirection(downFX, Vector3.down);
                break;
            case PieceType.ROW_BOMB:
                GameObject leftFX = InstantiateFX(piece, _lineBombFXPrefab);
                GameObject rightFX = InstantiateFX(piece, _lineBombFXPrefab);

                SetSpeedAndDirection(leftFX, Vector3.left);
                SetSpeedAndDirection(rightFX, Vector3.right);
                break;
            default:
                InstantiateFX(piece, _pieceClearFXPrefab);
                break;
        }

    }

    private void SetSpeedAndDirection(GameObject FXPrefab, Vector3 direction)
    {
        ParticleMover particleMover = FXPrefab.GetComponentInChildren<ParticleMover>();
        particleMover.SetSpeed(_lineBombSpeed);
        particleMover.SetDirection(direction);
    }

    private GameObject InstantiateFX(Piece piece, GameObject prefab)
    {
        GameObject gameObjectFX = Instantiate(prefab, new Vector3(piece.GetX(), piece.GetY(), 0), Quaternion.identity);
        gameObjectFX.GetComponent<Particle>().SetPieceSpriteColor(piece.GetPieceColor().ToColor());
        return gameObjectFX;
    }

    private void CellBreaker_OnCellBreaker(Cell cell)
    {
        GameObject cellBreakFX = Instantiate(_cellBreakFXPrefab, new Vector3(cell.GetCoordinate().x, cell.GetCoordinate().y, 0), Quaternion.identity);
    }
}