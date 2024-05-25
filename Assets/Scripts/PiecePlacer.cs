using UnityEngine;

public class PiecePlacer : MonoBehaviour
{
    [SerializeField]
    private PieceSO[] _piecesSO;

    public void PlacePieceAt(Board board, Cell cell, Piece piece)
    {
        piece.transform.position = cell.GetCoordinate();
        piece.transform.rotation = Quaternion.identity;
        piece.transform.parent = transform;
        piece.gameObject.name = "Piece: " + cell.GetCoordinate().x + "." + cell.GetCoordinate().y + " - " + piece.gameObject.name;

        piece.SetBoard(board);

        if (board.IsWithinBounds((int)cell.GetCoordinate().x, (int)cell.GetCoordinate().y))
        {
            cell.SetPiece(piece);
        }
    }

    public void PlacePieceAt(Board board, int x, int y, Piece piece)
    {
        PlacePieceAt(board, board.GetCellGrid()[x, y], piece);
    }

    public Piece PlaceRandomPieceAt(Board board, Cell cell)
    {
        GameObject randomPieceGO = GetRandomPieceGameObject();
        Piece piece = randomPieceGO.GetComponent<Piece>();
        PlacePieceAt(board, cell, piece);
        return piece;
    }

    public void SwitchPieceWithAnotherRandom(Board board, Cell cell)
    {
        cell.ClearPiece();
        PlaceRandomPieceAt(board, cell);
    }

    private GameObject GetRandomPieceGameObject()
    {
        int randomIndex = Random.Range(0, _piecesSO.Length);

        GameObject piecePrefab = Instantiate(_piecesSO[randomIndex]._piecePrefab);
        piecePrefab.GetComponent<Piece>().SetPieceColor(_piecesSO[randomIndex]._pieceColor);

        return piecePrefab;
    }
}