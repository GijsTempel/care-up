using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (prefs.tutorialCompleted || prefs.TutorialPopUpDeclined)
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
        loadingScreen.LoadLevel("UMenuPro");
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

    public void OnToggleAcceptTermsAndConditions(Button button)
    {
        button.interactable = !button.interactable;
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void OnTutorialButtonClick_Interface()
    {
        string sceneName = "Tutorial_UI";
        string bundleName = "tutorial_ui";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_Movement()
    {
        string sceneName = "Tutorial_Movement";
        string bundleName = "tutorial_move";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_Picking()
    {
        string sceneName = "Tutorial_Picking";
        string bundleName = "tutorial_pick";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_Combining()
    {
        string sceneName = "Tutorial_Combining";
        string bundleName = "tutorial_combine";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_UsingOn()
    {
        string sceneName = "Tutorial_UseOn";
        string bundleName = "tutorial_useon";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_PersonDialogues()
    {
        string sceneName = "Tutorial_Talk";
        string bundleName = "tutorial_talking";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_Sequences()
    {
        string sceneName = "Tutorial_Sequence";
        string bundleName = "tutorial_sequences";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }

    public void OnTutorialButtonClick_Theory()
    {
        string sceneName = "Tutorial_Theory";
        string bundleName = "tutorial_theory";
        bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName, bundleName);
    }
}
