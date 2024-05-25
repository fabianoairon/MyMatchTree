using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PieceSO))]
public class PieceSOEditor : Editor
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
                GUILayout.Label(preview, GUILayout.Height(100), GUILayout.Width(100));
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