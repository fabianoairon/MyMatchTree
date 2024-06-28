using UnityEngine;

public enum PieceColor
{
    BLUE,
    RED,
    GREEN,
    YELLOW,
    LIGHT_BLUE
}

public static class PieceColorExtensions
{
    public static Color ToColor(this PieceColor color, Board board)
    {
        switch (color)
        {
            case PieceColor.BLUE:
                return board.GetBlue();
            case PieceColor.RED:
                return board.GetRed();
            case PieceColor.GREEN:
                return board.GetGreen();
            case PieceColor.YELLOW:
                return board.GetYellow();
            case PieceColor.LIGHT_BLUE:
                return board.GetLightBlue();
            default:
                return Color.clear;
        }
    }
}