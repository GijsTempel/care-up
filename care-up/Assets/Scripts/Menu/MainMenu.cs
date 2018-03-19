using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    
    private LoadingScreen loadingScreen;
    private PlayerPrefsManager prefs;
	public string eMail="info@triplemotion.nl";
    
    private void Start()
    {
        if (GameObject.Find("Preferences") != null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");

            prefs = GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>();
        }
        else
        {
            Debug.LogWarning("No 'preferences' found. Game needs to be started from first scene");
        }
    }

    public void OnStartButtonClick()
    {
        if (prefs.TutorialCompleted || prefs.TutorialPopUpDeclined)
        {
            loadingScreen.LoadLevel("SceneSelection");
        }
        else
        {
            GameObject canvas = GameObject.Find("Canvas");

            canvas.transform.Find("MainMenu").gameObject.SetActive(false);
            //canvas.transform.Find("Logo").gameObject.SetActive(false);
           // canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);

            canvas.transform.Find("TutorialPopUp").gameObject.SetActive(true);
        }
    }

    public void OnStartYes()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Tutorial");
    }

    public void OnStartNo()
    {
        prefs.TutorialPopUpDeclined = true;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("SceneSelection");
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
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Tutorial", "tutorial");
    }

    public void OnOptionsButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(false);
		canvas.transform.Find("MainMenu").gameObject.SetActive(false);
        //canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);
        canvas.transform.Find("Opties").gameObject.SetActive(true);
    }

    public void OnOptionsBackButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(true);
		canvas.transform.Find("MainMenu").gameObject.SetActive(true);
        //canvas.transform.Find("OptionsBtn").gameObject.SetActive(true);
        canvas.transform.Find("Opties").gameObject.SetActive(false);
    }

    public void OnControlsButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(false);
		canvas.transform.Find("MainMenu").gameObject.SetActive(false);
       // canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);
        canvas.transform.Find("ControlsUI").gameObject.SetActive(true);
    }

    public void OnControlsCloseButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(true);
		canvas.transform.Find("MainMenu").gameObject.SetActive(true);
        //canvas.transform.Find("OptionsBtn").gameObject.SetActive(true);
        canvas.transform.Find("ControlsUI").gameObject.SetActive(false);
    }

    public void OnBugReportButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(false);
		canvas.transform.Find("MainMenu").gameObject.SetActive(false);
        //canvas.transform.Find("OptionsBtn").gameObject.SetActive(false);
        canvas.transform.Find("BugReportUI").gameObject.SetActive(true);
    }

    public void OnBugReportCloseButtonClick()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //canvas.transform.Find("MainMenu").gameObject.SetActive(true);
		canvas.transform.Find("MainMenu").gameObject.SetActive(true);
        //canvas.transform.Find("OptionsBtn").gameObject.SetActive(true);
        canvas.transform.Find("BugReportUI").gameObject.SetActive(false);

    }

	public void OnSendEmail()
	{
		System.Diagnostics.Process.Start (("mailto:" + eMail + "?subject=" + "Fout melding Care-Up."
		+ "&body="
		));
        GameObject.Find("MessageWindow").GetComponent<TimedPopUp>().Set("Uw mailprogramma wordt geopend.");
    }

    public void OnRetryButtonClick()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel(loadingScreen.GetComponent<EndScoreManager>().SceneName);
    }
}
