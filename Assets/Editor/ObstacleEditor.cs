using UnityEditor;
using UnityEngine;

public class ObstacleEditor : EditorWindow
{
    ObstacleGridArraySO obstacleGridArraySO;
    string path = "Assets/ObstacleGridArraySO.asset";

    [MenuItem("Tools/Obstacle Custom Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>("Obstacle Editor");
    }

    void OnGUI()
    {
        obstacleGridArraySO = (ObstacleGridArraySO)AssetDatabase.LoadAssetAtPath(path, typeof(ObstacleGridArraySO));
        
        if (obstacleGridArraySO == null)
        {
            EditorGUILayout.LabelField("ObstacleGridArraySO not found at " + path);
            return;
        }

        DisplayGridEditor();
    }
    
    private void DisplayGridEditor()
    {
        for (int i = 0; i < 10; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 10; j++)
            {
                Vector2Int gridPosition = new Vector2Int(i, j);
                bool buttonState = EditorGUILayout.Toggle(obstacleGridArraySO.data.Contains(gridPosition));
                if (buttonState)
                {
                    if (!obstacleGridArraySO.data.Contains(gridPosition))
                    {
                        obstacleGridArraySO.data.Add(gridPosition);
                    }
                }
                else
                {
                    if (obstacleGridArraySO.data.Contains(gridPosition))
                    {
                        obstacleGridArraySO.data.Remove(gridPosition);
                    }
                }
                EditorUtility.SetDirty(obstacleGridArraySO);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}