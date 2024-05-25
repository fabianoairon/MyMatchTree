using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    private Piece _selectedPiece;
    private Piece _swappedPiece;

    private bool _playerCanSwap = true;

    private List<Piece> _filledPieces;

    public bool IsAdjascent(Cell startCell, Cell endCell)
    {
        return Math.Abs(startCell.GetCoordinate().x - endCell.GetCoordinate().x) == 1 && startCell.GetCoordinate().y == endCell.GetCoordinate().y ||
        Math.Abs(startCell.GetCoordinate().y - endCell.GetCoordinate().y) == 1 && startCell.GetCoordinate().x == endCell.GetCoordinate().x;
    }

    public void SetFirstPiece(Piece piece)
    {
        if (_selectedPiece == null && _swappedPiece == null)
        {
            _selectedPiece = piece;
        }
    }

    public void SetSecondPiece(Piece piece)
    {
        if (piece.GetCell() == null) return;

        if (_selectedPiece != null && _swappedPiece == null)
        {
            _swappedPiece = piece;
        }
    }

    public void SwapPieces(Board board)
    {
        if (_playerCanSwap && _selectedPiece != null && _swappedPiece != null && IsAdjascent(_selectedPiece.GetCell(), _swappedPiece.GetCell()))
        {
            StartCoroutine(SwapPiecesRoutine(board, _selectedPiece, _swappedPiece));
        }

        _selectedPiece = null;
        _swappedPiece = null;
    }

    private IEnumerator SwapPiecesRoutine(Board board, Piece startPiece, Piece endPiece)
    {
        float swapDuration = board.GetMoveDuration(MoveType.SWAP, startPiece.GetCell().GetCoordinate(), endPiece.GetCell().GetCoordinate());

        if (startPiece != null && endPiece != null)
        {
            Swaping(startPiece, endPiece);
        }

        yield return new WaitForSeconds(swapDuration);

        var allMatches = board.GetMatcher().FindAllMatchesTwoPieces(board, startPiece, endPiece);

        if (allMatches.Count == 0)
        {
            Swaping(startPiece, endPiece);
        }
        else
        {
            board.ClearCollapseRefillMatch(allMatches);
        }
    }

    public void Swaping(Piece startPiece, Piece endPiece)
    {
        if (startPiece == null || endPiece == null) return;
        startPiece.Move(endPiece.GetCell().GetCoordinate(), MoveType.SWAP);
        endPiece.Move(startPiece.GetCell().GetCoordinate(), MoveType.SWAP);
    }

    public void SetPlayerCanSwap(bool boolean)
    {
        _playerCanSwap = boolean;
    }
}