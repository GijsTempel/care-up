using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat_CurrentAction : MonoBehaviour
{

    private ActionManager actionManager;

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");
    }

    void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), 
            actionManager.CurrentDescription, style);
    }
}
