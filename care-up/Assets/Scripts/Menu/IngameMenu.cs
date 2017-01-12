using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    }

    void Update()
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
                        SceneManager.LoadScene("Options");
                        break;
                    case "Exit":
                        SceneManager.LoadScene("Menu");
                        break;
                }
            }
        }
    }
}
