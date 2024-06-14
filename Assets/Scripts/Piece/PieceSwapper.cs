using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceSwapper : MonoBehaviour
{
    private Piece _startPiece;
    private Piece _targetPiece;

    private bool _playerCanSwap = true;

    private List<Piece> _filledPieces;

    public bool IsAdjascent(Cell startCell, Cell endCell)
    {
        return Math.Abs(startCell.GetCoordinate().x - endCell.GetCoordinate().x) == 1 && startCell.GetCoordinate().y == endCell.GetCoordinate().y ||
        Math.Abs(startCell.GetCoordinate().y - endCell.GetCoordinate().y) == 1 && startCell.GetCoordinate().x == endCell.GetCoordinate().x;
    }

    public void SetFirstPiece(Piece piece)
    {
        if (_startPiece == null && _targetPiece == null)
        {
            _startPiece = piece;
        }
    }

    public void SetSecondPiece(Piece piece)
    {
        if (piece.GetCell() == null) return;

        if (_startPiece != null && _targetPiece == null)
        {
            _targetPiece = piece;
        }
    }

    public void SwapPieces(Board board)
    {
        if (_playerCanSwap && _startPiece != null && _targetPiece != null && IsAdjascent(_startPiece.GetCell(), _targetPiece.GetCell()))
        {
            StartCoroutine(SwapPiecesRoutine(board, _startPiece, _targetPiece));
        }

        _startPiece = null;
        _targetPiece = null;
    }

    private IEnumerator SwapPiecesRoutine(Board board, Piece startPiece, Piece endPiece)
    {
        float swapDuration = board.GetMoveDuration(MoveType.SWAP, startPiece.GetCell().GetCoordinate(), endPiece.GetCell().GetCoordinate());

        if (startPiece != null && endPiece != null)
        {
            Swaping(startPiece, endPiece);
        }

        yield return new WaitForSeconds(swapDuration);

        SetSwappers(startPiece, endPiece);
        var allMatches = board.GetMatcher().FindAllMatchesTwoPieces(board, startPiece, endPiece);

        if (allMatches.Count == 0)
        {
            Swaping(startPiece, endPiece);
            UnsetSwappers(startPiece, endPiece);
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

    private void SetSwappers(Piece startPiece, Piece endPiece)
    {
        startPiece.SetIsSwapping(true);
        endPiece.SetIsSwapping(true);
    }

    private void UnsetSwappers(Piece startPiece, Piece endPiece)
    {
        startPiece.SetIsSwapping(false);
        endPiece.SetIsSwapping(false);
    }

    public void UnsetSwappers(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            if (piece.IsSwapping())
            {
                piece.SetIsSwapping(false);
            }
        }
    }
}