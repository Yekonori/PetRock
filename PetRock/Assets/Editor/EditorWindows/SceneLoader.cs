using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : EditorWindow
{
    #region Fields

    private List<int> _loadedScenes = new List<int>();

    #endregion Fields

    #region Constants

    private const float GROUP_GAP = 50f;

    #endregion Fields

    [MenuItem("Tools/SceneLoader")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneLoader));
    }

    #region Unity Methods

    private void Awake()
    {
        EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0), OpenSceneMode.Single);

        _loadedScenes.Add(0);
    }

    private void OnGUI()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        float windowWidth = position.width;

        for (int i = 1; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

            if (GUI.Button(new Rect(10, 15 + (i - 1) * GROUP_GAP, position.width - 30, 45), scenes[i]))
            {
                if (_loadedScenes.Contains(i))
                {
                    EditorSceneManager.CloseScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(i), true);

                    _loadedScenes.Remove(i);
                }
                else
                {
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i), OpenSceneMode.Additive);

                    _loadedScenes.Add(i);
                }
            }
        }
    }

    #endregion Unity Methods
}
