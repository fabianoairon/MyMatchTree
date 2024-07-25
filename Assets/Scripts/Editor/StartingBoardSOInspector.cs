using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CustomEditor(typeof(StartingBoardSO))]
public class StartingBoardSOInspector : Editor
{
    private StartingBoardSO startingBoardSO;
    private PieceSO selectedPiece;
    private CellSO selectedCell;
    private GUIStyle buttonStyle;
    private const int buttonSize = 30;

    private void OnEnable()
    {
        startingBoardSO = (StartingBoardSO)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = buttonSize,
                fixedHeight = buttonSize,
                padding = new RectOffset(0, 0, 0, 0)
            };
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Width");
        EditorGUILayout.LabelField("Height");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        startingBoardSO.boardWidth = EditorGUILayout.IntSlider(startingBoardSO.boardWidth, 0, 15);
        startingBoardSO.boardHeight = EditorGUILayout.IntSlider(startingBoardSO.boardHeight, 0, 15);
        EditorGUILayout.EndHorizontal();

        if (startingBoardSO.AllPiecesAndCells != null)
        {
            int margin = 3;
            EditorGUILayout.LabelField("Select a Piece:");
            float windowWidth = EditorGUIUtility.currentViewWidth;
            int buttonsPerRow = Mathf.FloorToInt((windowWidth - margin) / (buttonSize + margin));

            if (buttonsPerRow == 0) buttonsPerRow = 1;

            EditorGUILayout.BeginHorizontal();

            int pieceCount = 0;
            foreach (var piece in startingBoardSO.AllPiecesAndCells.pieces)
            {
                if (pieceCount % buttonsPerRow == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }

                GameObject gameObject = piece._piecePrefab;

                bool isSelected = selectedPiece == piece;

                Color originalBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = isSelected ? Color.cyan : originalBackgroundColor;

                if (GUILayout.Button(piece._pieceTextureIcon, buttonStyle))
                {
                    selectedPiece = piece;
                    Debug.Log("Selected Piece: " + gameObject.name + " " + piece._pieceColor);
                }
                pieceCount++;
                GUI.backgroundColor = originalBackgroundColor;
            }

            if (GUILayout.Button("", buttonStyle))
            {
                selectedPiece = null;
                Debug.Log("Selected Piece: null");
            }

            EditorGUILayout.EndHorizontal();



            EditorGUILayout.LabelField("Select a Cell:");
            EditorGUILayout.BeginHorizontal();

            foreach (var cell in startingBoardSO.AllPiecesAndCells.cells)
            {
                GameObject gameObject = cell._cellPrefab;

                bool isSelected = selectedCell == cell;


                Color originalBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = isSelected ? Color.cyan : originalBackgroundColor;

                if (GUILayout.Button(cell._cellTextureIcon, buttonStyle))
                {
                    selectedCell = cell;
                    Debug.Log("Selected Cell Type: " + gameObject.name + " " + (cell._breakableValue == 0 ? "" : cell._breakableValue));
                }

                GUI.backgroundColor = originalBackgroundColor;
            }

            if (GUILayout.Button("", buttonStyle))
            {
                selectedCell = null;
                Debug.Log("Selected Cell: null");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Board Grid:");
        }
        else
        {
            EditorGUILayout.HelpBox("Please set \"All Pieces And Cells\" SO to see all possible Pieces and Cells!", MessageType.Warning);
        }

        List<StartingPiece> listStartingPiece = startingBoardSO.StartingPiecesSet.ToList();
        List<StartingCell> listStartingCell = startingBoardSO.StartingCellsSet.ToList();

        if (startingBoardSO.boardWidth == 0 || startingBoardSO.boardHeight == 0)
        {
            EditorGUILayout.HelpBox("Both WIDTH and HEIGHT values must be greater than ZERO!", MessageType.Error);
        }
        else if (startingBoardSO.boardWidth > 0 || startingBoardSO.boardHeight > 0)
        {
            for (int i = startingBoardSO.boardHeight - 1; i >= 0; i--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < startingBoardSO.boardWidth; j++)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(buttonSize, buttonSize, buttonStyle);


                    StartingPiece currentPiece = listStartingPiece.FirstOrDefault(p => p.X == j && p.Y == i);
                    StartingCell currentCell = listStartingCell.FirstOrDefault(p => p.X == j && p.Y == i);

                    if (GUI.Button(buttonRect, GUIContent.none, buttonStyle))
                    {
                        if (selectedPiece != null && currentPiece == null)
                        {
                            listStartingPiece.Add(new StartingPiece(selectedPiece, j, i));
                        }
                        else if (selectedPiece != null && currentPiece != null)
                        {
                            listStartingPiece.Remove(currentPiece);
                            listStartingPiece.Add(new StartingPiece(selectedPiece, j, i));
                        }
                        else if (selectedPiece == null && currentPiece != null)
                        {
                            listStartingPiece.Remove(currentPiece);
                        }




                        if (selectedCell != null && currentCell == null)
                        {
                            listStartingCell.Add(new StartingCell(selectedCell, j, i));
                        }
                        else if (selectedCell != null && currentCell != null)
                        {
                            listStartingCell.Remove(currentCell);
                            listStartingCell.Add(new StartingCell(selectedCell, j, i));
                        }
                        else if (selectedCell == null && currentCell != null)
                        {
                            listStartingCell.Remove(currentCell);
                        }
                    }

                    if (currentCell != null)
                    {
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), currentCell.CellSO._cellTextureIcon);
                    }

                    if (currentPiece != null)
                    {
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), currentPiece.PieceSO._pieceTextureIcon);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        for (int k = listStartingPiece.Count - 1; k >= 0; k--)
        {
            var sPiece = listStartingPiece[k];

            if (sPiece.X >= startingBoardSO.boardWidth || sPiece.Y >= startingBoardSO.boardHeight)
            {
                listStartingPiece.RemoveAt(k);
                Debug.Log("Piece removed: " + sPiece.PieceSO._piecePrefab.name);
            }
        }

        for (int k = listStartingCell.Count - 1; k >= 0; k--)
        {
            var sCell = listStartingCell[k];

            if (sCell.X >= startingBoardSO.boardWidth || sCell.Y >= startingBoardSO.boardHeight)
            {
                listStartingCell.RemoveAt(k);
                Debug.Log("Cell removed: " + sCell.CellSO._cellPrefab.name);
            }
        }

        startingBoardSO.StartingPiecesSet = listStartingPiece.ToArray();
        startingBoardSO.StartingCellsSet = listStartingCell.ToArray();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(startingBoardSO);
        }
    }
}
