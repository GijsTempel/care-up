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
