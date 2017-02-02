using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    public Texture loadingTexture;
    public string loadingText;

    private string sceneToLoad;
    private bool loading = false;
    
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

    public void LoadLevel(string sceneName)
    {
        sceneToLoad = sceneName;
        loading = true;
        StartCoroutine(LoadNewScene());
    }
}
