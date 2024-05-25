using UnityEngine;

public class Cell : MonoBehaviour
{
    private int _xCoordinate;
    private int _yCoordinate;

    private Board _board;
    private Piece _piece;

    public void Setup(int x, int y, Board board)
    {
        _xCoordinate = x;
        _yCoordinate = y;
        _board = board;
        gameObject.name = "Cell " + _xCoordinate + "." + _yCoordinate;
    }

    public void SetPiece(Piece piece)
    {
        _piece = piece;

        if (_piece != null)
        {
            _piece.SetCell(this);
        }
    }

    public void ClearPiece()
    {
        var piece = _piece;
        _board.HightlightPieceOff(piece);
        Destroy(piece.gameObject);
        _piece = null;
    }

    private void OnMouseDown()
    {
        if (_piece == null) return;
        _board.GetSwapper().SetFirstPiece(_piece);
    }

    private void OnMouseEnter()
    {
        if (_piece == null) return;
        _board.GetSwapper().SetSecondPiece(_piece);
    }

    private void OnMouseUp()
    {
        _board.GetSwapper().SwapPieces(_board);
    }

    public Board GetBoard()
    {
        return _board;
    }

    public Vector3 GetCoordinate()
    {
        return new Vector2(_xCoordinate, _yCoordinate);
    }

    public Piece GetPiece()
    {
        return _piece;
    }
}
