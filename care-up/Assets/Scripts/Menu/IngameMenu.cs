using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour {

    private GameObject UIFolder;
    private PlayerMovement playerScript;

    private static Controls controls;
    private static GameTimer timer;

	void Start () {
        UIFolder = GameObject.Find("InGameOptions");

        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (playerScript == null) Debug.LogError("No Player script found");

        if (controls == null)
        {
            controls = GameObject.Find("GameLogic").GetComponent<Controls>();
            if (controls == null) Debug.LogError("No controls found");
        }

        if (timer == null)
        {
            timer = GameObject.Find("GameLogic").GetComponent<GameTimer>();
            if (timer == null) Debug.LogError("No timer found");
        }
    }

    public void Open()
    {
        UIFolder.transform.GetChild(0).gameObject.SetActive(true);
        controls.enabled = false;
        playerScript.enabled = false;
        timer.enabled = false;
    }

    public void Close()
    {
        UIFolder.transform.GetChild(0).gameObject.SetActive(false);
        controls.enabled = true;
        playerScript.enabled = true;
        timer.enabled = true;
    }

    public void OnContinue()
    {
        Close();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSave()
    {
        Debug.Log("Ingame::Save");
    }

    public void OnOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnExit()
    {
        SceneManager.LoadScene("Menu");
    }
}
