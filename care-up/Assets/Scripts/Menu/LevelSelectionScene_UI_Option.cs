﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScene_UI_Option : MonoBehaviour {

    public Color selectedColor = Color.green;

    private bool selected = false;

    public string sceneName;
    public string description;
    public string result;
    
    public void SetSelected()
    {
        // turn on
        if (!selected)
        {
            transform.parent.Find("Start").GetComponent<LevelButton>().sceneName = sceneName;
            
            transform.parent.Find("Description").GetComponent<Text>().text = description;
            transform.parent.Find("Result").GetComponent<Text>().text = result;
            
            LevelSelectionScene_UI_Option[] other = transform.parent.GetComponentsInChildren<LevelSelectionScene_UI_Option>();
            foreach (LevelSelectionScene_UI_Option ui in other)
            {
                ui.GetComponent<Image>().color = Color.white;
                ui.selected = false;
            }

            GetComponent<Image>().color = selectedColor;

            selected = true;
        }
    }
}
