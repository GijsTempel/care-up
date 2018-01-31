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
    private float delayTimer = 0.0f;
    
    // Set some temporary texture and text
    void Start()
    {
        loadingTexture = Resources.Load<Texture>("Sprites/Menu_background");
        loadingText = "Laden...";
     
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!async.isDone)
        {
            delayTimer += Time.deltaTime;
            yield return null;
        }

        if (delayTimer < 1.0f)
        {
            yield return new WaitForSeconds(1.0f - delayTimer);
        }

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
            style.fontSize = 22;
            GUI.color = Color.black;
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
        Time.timeScale = 1.0f;

        sceneToLoad = sceneName;
        loading = true;
        delayTimer = 0.0f;
        StartCoroutine(LoadNewScene());
    }
}
