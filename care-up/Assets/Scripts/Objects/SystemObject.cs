using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Object that loads certain scene or performs an action, not related to gameplay.
/// </summary>
public class SystemObject : InteractableObject {

    public string sceneName;

    private LoadingScreen loadingScreen;

    protected override void Start()
    {
        base.Start();

        loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
        if (loadingScreen == null) Debug.LogError("No loading screen found");
    }

    protected override void Update()
    {
        base.Update();

        if (controls.MouseClicked() && controls.SelectedObject == gameObject && controls.CanInteract )
        {
            Use();
        }
    }

    // called when player interacts with object
    public virtual void Use(bool confirmed = false)
    {
        if (name == "Start")
        {
            if (!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().TutorialCompleted)
            {
                if (GameObject.Find("_SkipTutorial"))
                {
                    Debug.LogWarning("TutorialSkip found! => Skipping tutorial");
                    loadingScreen.LoadLevel("SceneSelection");
                    //SceneManager.LoadScene("SceneSelection");
                }
                else
                {
                    Debug.Log("Tutorial is not completed.");
                    loadingScreen.LoadLevel("Tutorial");
                    //SceneManager.LoadScene("Tutorial");
                }
            }
            else
            {
                Debug.Log("Tutorial is completed.");
                loadingScreen.LoadLevel("SceneSelection");
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
                if (confirmed || sceneName == "Tutorial" || sceneName == "Options" || sceneName == "Menu")
                {
                    loadingScreen.LoadLevel(sceneName);
                    //SceneManager.LoadScene(sceneName);
                }
                else {
                    cameraMode.ToggleCameraMode(CameraMode.Mode.ConfirmUI);
                }
            }
        }
    }

    protected override void SetShaderTo(Shader shader)
    {
        foreach (Material m in rend.materials)
        {
            m.shader = shader;
        }
    }
}
