using UnityEngine;

public enum PieceColor
{
    BLUE,
    RED,
    GREEN,
    YELLOW,
    LIGHT_BLUE,
    WILD
}

public static class PieceColorExtensions
{
    public static Color ToColor(this PieceColor color)
    {
        switch (color)
        {
            case PieceColor.BLUE:
                return Color.blue;
            case PieceColor.RED:
                return Color.red;
            case PieceColor.GREEN:
                return Color.green;
            case PieceColor.YELLOW:
                return Color.yellow;
            case PieceColor.LIGHT_BLUE:
                return Color.cyan;
            default:
                return Color.black;
        }
    }
}