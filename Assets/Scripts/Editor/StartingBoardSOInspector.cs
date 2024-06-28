using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartingBoardSO))]
public class StartingBoardSOInspector : Editor
{
    public override void OnInspectorGUI()
    {
        StartingBoardSO startingBoardSO = (StartingBoardSO)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Width");
        EditorGUILayout.LabelField("Height");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        startingBoardSO.boardWidth = EditorGUILayout.IntSlider(startingBoardSO.boardWidth, 0, 15);
        startingBoardSO.boardHeight = EditorGUILayout.IntSlider(startingBoardSO.boardHeight, 0, 15);
        EditorGUILayout.EndHorizontal();


        // Desenhar um retângulo
        Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        EditorGUI.DrawRect(rect, Color.gray);


        // Detectar clique no retângulo
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0 && rect.Contains(e.mousePosition))
        {
            Debug.Log("Rect clicked!");
            // Marcar o evento como usado para evitar propagação
            e.Use();
        }
    }
}
