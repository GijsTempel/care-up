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
            if ( !GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().TutorialCompleted )
            {
                Debug.LogWarning("Tutorial is not completed. Load tutorial level now.");
                Debug.LogWarning("No tutorial level yet, so loading module selection.");
                SceneManager.LoadScene("SceneSelection");
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
