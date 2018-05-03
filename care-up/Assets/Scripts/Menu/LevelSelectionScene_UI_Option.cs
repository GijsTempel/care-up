using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScene_UI_Option : MonoBehaviour {

    public Color selectedColor = Color.green;

    private bool selected = false;

    public string bundleName;
    public string sceneName;
    public string description;
    public string result;
    public Sprite image;
    
    public void SetSelected()
    {
        // turn on
        if (!selected)
        {
            LevelButton levelButton = transform.parent.Find("Start").GetComponent<LevelButton>();

            levelButton.sceneName = sceneName;
            levelButton.bundleName = bundleName;

            levelButton.UpdateLeaderBoard();
            levelButton.UpdateHighScore();

            transform.parent.Find("Description").GetComponent<Text>().text = description;
            transform.parent.Find("Result").GetComponent<Text>().text = result;
            
            LevelSelectionScene_UI_Option[] other = transform.parent.GetComponentsInChildren<LevelSelectionScene_UI_Option>();
            foreach (LevelSelectionScene_UI_Option ui in other)
            {
                ui.GetComponent<Image>().color = Color.white;
                ui.selected = false;
            }

            GetComponent<Image>().color = selectedColor;

            transform.parent.Find("Image").GetComponent<Image>().sprite = image;

            selected = true;
        }
    }
}
