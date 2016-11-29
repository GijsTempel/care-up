using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.transform.name)
                {
                    case "Start":
                        OnStart();
                        break;
                    case "Continue":
                        OnContinue();
                        break;
                    case "Options":
                        OnOptions();
                        break;
                    case "Exit":
                        OnExit();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnStart()
    {
        SceneManager.LoadScene("Selection");
    }

    void OnContinue()
    {
        GameObject.Find("Preferences").GetComponent<SaveLoadManager>().LoadLevel();
    }

    void OnOptions()
    {
        SceneManager.LoadScene("Options");
    }

    void OnExit()
    {
        Application.Quit();
    }
}
