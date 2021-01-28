using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles ingame object as small menu.
/// Save game | Options | Exit
/// </summary>
public class IngameMenu : MonoBehaviour
{

    private PlayerMovement playerScript;

    private static Controls controls;
    private static GameTimer timer;

    void Start()
    {
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
        /*
        if (GameObject.FindObjectOfType<LoginProAsset.LoginPro_Manager>() == null)
        {
            gameObject.AddComponent<LoginProAsset.LoginPro_Manager>();
        }*/
           
    }

    // not used for ages
    /*void Update()
    {
        if (controls.MouseClicked())
        {
            if (controls.SelectedObject)
            {
                switch (controls.SelectedObject.name)
                {
                    case "Save":
                        GameObject.Find("Preferences").GetComponent<SaveLoadManager>().Save();
                        break;
                    case "Options":
                        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("Options");
                        //SceneManager.LoadScene("Options");
                        break;
                    case "Exit":
                        GameObject.Find("Preferences").GetComponent<LoadingScreen>().LoadLevel("UMenuPro");
                        //SceneManager.LoadScene("Menu");
                        break;
                }
            }
        }
    }*/
}
