using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour {

    private static LoadingScreen loadingScreen;

    public string sceneName;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null && loadingScreen == null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");
        }
    }

    public void OnLevelButtonClick()
    {
        loadingScreen.LoadLevel(sceneName);
    }

}
