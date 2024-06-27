using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PieceSO))]
public class PieceSOInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PieceSO piecesSO = (PieceSO)target;

        if (piecesSO._piecePrefab != null)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(piecesSO._piecePrefab);

            if (preview != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(preview, GUILayout.Height(100), GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("No preview available");
            }
        }
        else
        {
            GUILayout.Label("No prefab assigned");
        }
    }
}