using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool _isMoving = false;
    private bool _isSwapping = false;
    private Cell _cell;

    [SerializeField]
    private PieceColor _pieceColor;

    public void Move(Vector3 destination, MoveType moveType)
    {
        if (!_isMoving)
        {
            StartCoroutine(MoveRoutine(destination, moveType));
        }
        else
        {
            Debug.LogWarning("Cannot move!!!");
        }
    }

    private IEnumerator MoveRoutine(Vector3 destination, MoveType moveType)
    {
        if (_cell.GetBoard().GetDebugLogManager().StartAndEndCoroutines) Debug.Log("Piece.MoveRoutine Started");
        bool isInDestination = false;
        float currentTime = 0;
        Vector3 initialPosition = transform.position;

        float moveDuration = _cell.GetBoard().GetMoveDuration(moveType, initialPosition, destination);


        _isMoving = true;
        while (!isInDestination)
        {
            if (Vector3.Distance(transform.position, destination) < 0.01f || moveType == MoveType.INSTANT)
            {
                _cell.GetBoard().GetPiecePlacer().PlacePieceAt(_cell.GetBoard(), (int)destination.x, (int)destination.y, this);
                _isMoving = false;
                break;
            }
            else
            {
                currentTime += Time.deltaTime;

                float t = Mathf.Clamp(currentTime / moveDuration, 0f, 1f);
                t = t * t * (3f - 2f * t);
                transform.position = Vector3.Lerp(initialPosition, destination, t);

                yield return null;
            }
        }


        if (_cell.GetBoard().GetDebugLogManager().StartAndEndCoroutines) Debug.Log("Piece.MoveRoutine Ended");
    }

    public void SetPosition(int x, int y, int z = 0, int yOffSet = 0)
    {
        transform.position = new Vector3(x, y + yOffSet, z);
    }

    public Cell GetCell()
    {
        return _cell;
    }

    public void SetCell(Cell cell)
    {
        _cell = cell;
    }

    public int GetX()
    {
        return (int)_cell.GetCoordinate().x;
    }

    public int GetY()
    {
        return (int)_cell.GetCoordinate().y;
    }

    public void SetPieceColor(PieceColor pieceColor)
    {
        _pieceColor = pieceColor;
    }

    public PieceColor GetPieceColor()
    {
        return _pieceColor;
    }

    public Color GetPieceSpriteColor()
    {
        return GetComponent<SpriteRenderer>().color;
    }

    public void SetPieceSpriteColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public bool IsSwapping()
    {
        return _isSwapping;
    }

    public void SetIsSwapping(bool isSwapping)
    {
        _isSwapping = isSwapping;
    }

    public Piece SetColorAndSprite(Piece piece, PieceColor pieceColor, Color color)
    {
        SetPieceColor(pieceColor);
        SetPieceSpriteColor(color);
        return piece;
    }
}
