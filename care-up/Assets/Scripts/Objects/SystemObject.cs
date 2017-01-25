using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemObject : InteractableObject {

    public string sceneName;

    protected override void Update()
    {
        base.Update();

        if (controls.MouseClicked() && controls.SelectedObject == gameObject && controls.CanInteract )
        {
            Use();
        }
    }


    public virtual void Use()
    {
        if (name == "Start")
        {
            if (!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().TutorialCompleted)
            {
                if (GameObject.Find("_SkipTutorial"))
                {
                    Debug.LogWarning("TutorialSkip found! => Skipping tutorial");
                    SceneManager.LoadScene("SceneSelection");
                }
                else
                {
                    Debug.Log("Tutorial is not completed.");
                    SceneManager.LoadScene("Tutorial");
                }
            }
            else
            {
                if (GameObject.Find("_LaunchTutorial"))
                {
                    Debug.LogWarning("TutorialLaunch found! => forcing tutorial launch");
                    SceneManager.LoadScene("Tutorial");
                }
                else {
                    Debug.Log("Tutorial completed.");
                    SceneManager.LoadScene("SceneSelection");
                }
            }
        }
        else {
            if (sceneName == "_Continue")
            {
                GameObject.Find("Preferences").GetComponent<SaveLoadManager>().LoadLevel();
            }
            else if (sceneName == "_Exit")
            {
                Application.Quit();
            }
            else {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
