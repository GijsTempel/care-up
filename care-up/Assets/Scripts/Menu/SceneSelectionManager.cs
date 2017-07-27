using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelectionManager : MonoBehaviour {

    public SceneSelection CurrentMenu;

    public void Start()
    {
        ShowMenu(CurrentMenu);
    }

    public void ShowMenu (SceneSelection sceneselection)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;

            CurrentMenu = sceneselection;
            CurrentMenu.IsOpen = true;
        }
    }

}
