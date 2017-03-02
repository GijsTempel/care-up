using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle loading screen. 
/// </summary>
public class LoadingScreen : MonoBehaviour {

    // Texture to display during loading
    public Texture loadingTexture;
    // Text to display while loading
    public string loadingText;

    private string sceneToLoad;
    private bool loading = false;
    
    // Set some temporary texture and text
    void Start()
    {
        loadingTexture = Resources.Load<Texture>("Textures/loading_bg");
        loadingText = "Loading...";
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!async.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        loading = false;
    }

    /// <summary>
    /// Draw text and texture.
    /// </summary>
    void OnGUI()
    {
        if ( loading )
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingTexture, ScaleMode.StretchToFill);

            GUIStyle style = GUI.skin.GetStyle("Label");
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width / 2, Screen.height / 2),
            loadingText, style);
        }
    }

    /// <summary>
    /// Loads scene async, launches screen.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public void LoadLevel(string sceneName)
    {
        sceneToLoad = sceneName;
        loading = true;
        StartCoroutine(LoadNewScene());
    }
}
