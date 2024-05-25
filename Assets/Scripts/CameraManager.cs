using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Board _board;

    [SerializeField]
    private int _borderUnits;

    private void Start()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        int width = _board.GetWidth();
        int height = _board.GetHeight();

        float verticalOrtographicSize = ((float)_board.GetHeight() / 2) + _borderUnits;
        float horizontalOrtographicSize = (((float)_board.GetWidth() / 2) + _borderUnits) / Camera.main.aspect;


        Camera.main.orthographicSize = verticalOrtographicSize > horizontalOrtographicSize ?
            verticalOrtographicSize : horizontalOrtographicSize;

        transform.position = new Vector3((width - 1) / 2, (height - 1) / 2, -10);
    }
}
