using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGroupPageButton : MonoBehaviour
{
    public int pageID = -1;
    SceneGroupMenu sceneGroupMenu;

    public void ButtonClicked()
    {
        if (pageID < 0)
            return;
        if (sceneGroupMenu == null)
        {
            sceneGroupMenu = GameObject.FindObjectOfType<SceneGroupMenu>();
        }
        sceneGroupMenu.SwitchPage(pageID);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
