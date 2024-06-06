using UnityEngine;

public class BreakableCell : Cell
{
    private int currentBreakablePhase;

    public void BreakCell()
    {
        if (currentBreakablePhase > 1)
        {
            --currentBreakablePhase;
            ChangeSprite(currentBreakablePhase);
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
        currentBreakablePhase = phase;
        ChangeSprite(phase);
    }
}
