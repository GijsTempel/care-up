using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    private static LoadingScreen loadingScreen;

    public string sceneName;
    public bool multiple;

    private static Transform sceneInfoPanel;

    private void Start()
    {
        if (GameObject.Find("Preferences") != null && loadingScreen == null)
        {
            loadingScreen = GameObject.Find("Preferences").GetComponent<LoadingScreen>();
            if (loadingScreen == null) Debug.LogError("No loading screen found");
        }

        if (sceneInfoPanel == null)
        {
            sceneInfoPanel = GameObject.Find("SceneInfo").transform.GetChild(0);
            if (sceneInfoPanel == null) Debug.LogError("No sceneInfo panel");
        }
    }

    public void OnLevelButtonClick()
    {
        sceneInfoPanel.Find("Name").GetComponent<Text>().text = 
            transform.Find("Name").GetComponent<Text>().text;
        sceneInfoPanel.Find("Description").GetComponent<Text>().text = 
            transform.Find("Description").GetComponent<Text>().text;
        sceneInfoPanel.Find("Result").GetComponent<Text>().text = 
            transform.Find("Result").GetComponent<Text>().text;

        for (int i = 1; i <= 3; ++i)
        {
            string name = "Option_" + i;
            LevelSelectionScene_UI_Option to =
                sceneInfoPanel.Find(name).GetComponent<LevelSelectionScene_UI_Option>();
            LevelSelectionScene_UI_Option from =
                transform.Find(name).GetComponent<LevelSelectionScene_UI_Option>();

            if (multiple) {
                to.gameObject.SetActive(true);
            } else {
                to.gameObject.SetActive(false);
            }
            to.transform.Find("Text").GetComponent<Text>().text =
                    from.transform.Find("Text").GetComponent<Text>().text;

            to.sceneName = from.sceneName;
            to.description = from.description;
            to.result = from.result;
            to.image = from.image;

            if (i == 1 && multiple)
                to.SetSelected();
        }
    }

    public void OnStartButtonClick()
    {
        loadingScreen.LoadLevel(sceneName);
    }
}
