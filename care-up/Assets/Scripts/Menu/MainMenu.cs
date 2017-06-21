using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    
    private LoadingScreen loadingScreen;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");
        }
        else
        {
            Debug.LogWarning("No 'preferences' found. Game needs to be started from first scene");
        }
    }

    public void OnStartButtonClick()
    {
        loadingScreen.LoadLevel("Tutorial");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
