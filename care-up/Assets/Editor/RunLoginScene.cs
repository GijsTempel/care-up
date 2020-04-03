using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Timers;
using System.Collections;

class RunLoginScene : EditorWindow
{
    [MenuItem("Play/PlayMe _%h")]
    public static void RunMainScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            if (EditorSceneManager.GetActiveScene().path != "Assets/myBad Studios/WUSS/Demo/Login/LoginMenu.unity")
                PlayerPrefs.SetString("LastOpenedScene", EditorSceneManager.GetActiveScene().path);

            EditorSceneManager.OpenScene("Assets/myBad Studios/WUSS/Demo/Login/LoginMenu.unity");
            EditorApplication.isPlaying = true;
        }
    }
    [MenuItem("Play/LoadLastOpened _%#h")]
    public static void LoadLastOpenedScene()
    {
        if (!EditorApplication.isPlaying)
        {
            string LastOpenedScene = PlayerPrefs.GetString("LastOpenedScene");
            if (LastOpenedScene != null)
            {
                Debug.Log(Application.dataPath + LastOpenedScene.Substring(6));
                if (System.IO.File.Exists(Application.dataPath + LastOpenedScene.Substring(6)))
                {
                    EditorSceneManager.OpenScene(LastOpenedScene);
                    Debug.Log(LastOpenedScene);
                }
            }
        }
    }
}