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


    public virtual void Use(bool confirmed = false)
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
                if (confirmed || sceneName == "Tutorial" || sceneName == "Options")
                {
                    SceneManager.LoadScene(sceneName);
                }
                else {
                    cameraMode.ToggleCameraMode(CameraMode.Mode.ConfirmUI);
                }
            }
        }
    }
}
