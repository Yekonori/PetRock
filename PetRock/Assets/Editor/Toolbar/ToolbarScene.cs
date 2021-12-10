using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarScene : Editor
{
    static ToolbarScene()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("1", "Start Scene 1")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/LD1.unity");
        }

        if (GUILayout.Button(new GUIContent("4", "Start Scene 4")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/LD4.unity");
        }

        if (GUILayout.Button(new GUIContent("5", "Start Scene 5")))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/LD5.unity");
        }
    }
}
