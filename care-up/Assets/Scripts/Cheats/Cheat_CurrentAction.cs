using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Developer script. Attach to a dev GameObject, that will be disabled for a release version of the game.
/// Displays a label with a description of a current action on the top center of the screen.
/// </summary>
public class Cheat_CurrentAction : MonoBehaviour
{
    private ActionManager actionManager;
    
    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");
    }

    /// <summary>
    /// Displays a text message.
    /// </summary>
    void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("Label");
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 25;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), 
            actionManager.CurrentDescription, style);
    }
}
