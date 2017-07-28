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
        loadingScreen.LoadLevel("SceneSelection");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnMainMenuButtonClick()
    {
        loadingScreen.LoadLevel("Menu");
    }

    public void OnTutorialButtonClick()
    {
        loadingScreen.LoadLevel("Tutorial");
    }
    public void OnOptionsButtonClick()
    {
        loadingScreen.LoadLevel("Options");
    }
}
