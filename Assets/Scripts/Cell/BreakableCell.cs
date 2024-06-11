using System;
using UnityEngine;

public class BreakableCell : Cell
{
    public event Action<Cell> OnCellBroke;

    private int _currentBreakablePhase;

    public void BreakCell()
    {
        if (_currentBreakablePhase > 1)
        {
            --_currentBreakablePhase;
            ChangeSprite(_currentBreakablePhase);
        }
        else
        {
            _board.GetCellPlacer().ReplaceWithNewNormalCell(_board, this);
        }
    }

    private void ChangeSprite(int phase)
    {
        Sprite sprite = _board.GetCellPlacer().GetCurrentBreakableSprite(phase);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = _board.GetCellPlacer().GetBreakableCellColor();
    }

    public void SetPhase(int phase)
    {
        phase = phase == 0 ? 1 : phase; // prevent breakable cells placed with phase 0, that would be a normal cell

        _currentBreakablePhase = phase;
        ChangeSprite(phase);
    }

    public override void Setup(int x, int y, Board board, CellType cellType, int initialValue)
    {
        base.Setup(x, y, board, cellType);
        SetPhase(initialValue);
    }
}
