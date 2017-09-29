using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    private LoadingScreen loadingScreen;
	public string eMail="info@triplemotion.nl";

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
        Debug.Log("Exiting");
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

    public void OnControlsButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.transform.Find("MainMenu").gameObject.SetActive(false);
        canvas.transform.Find("Logo").gameObject.SetActive(false);
        canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);
        canvas.transform.Find("ControlsUI").gameObject.SetActive(true);
    }

    public void OnControlsCloseButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.transform.Find("MainMenu").gameObject.SetActive(true);
        canvas.transform.Find("Logo").gameObject.SetActive(true);
        canvas.transform.Find("OptionsBtn").gameObject.SetActive(true);
        canvas.transform.Find("ControlsUI").gameObject.SetActive(false);
    }

    public void OnBugReportButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.transform.Find("MainMenu").gameObject.SetActive(false);
        canvas.transform.Find("Logo").gameObject.SetActive(false);
        canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);
        canvas.transform.Find("BugReportUI").gameObject.SetActive(true);
    }

    public void OnBugReportCloseButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        canvas.transform.Find("MainMenu").gameObject.SetActive(true);
        canvas.transform.Find("Logo").gameObject.SetActive(true);
        canvas.transform.Find("OptionsBtn").gameObject.SetActive(true);
        canvas.transform.Find("BugReportUI").gameObject.SetActive(false);

    }

	public void OnSendEmail()
	{
		System.Diagnostics.Process.Start (("mailto:" + eMail + "?subject=" + "Fout melding Care-Up."
		+ "&body="
		));
	}

    public void OnRetryButtonClick()
    {
        loadingScreen.LoadLevel(loadingScreen.GetComponent<EndScoreManager>().SceneName);
    }
}
