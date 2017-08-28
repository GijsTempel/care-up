using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Developer script. Attach to a dev GameObject, that will be disabled for a release version of the game.
/// Displays a label with a description of a current action on the top center of the screen.
/// </summary>
public class Cheat_CurrentAction : MonoBehaviour
{
    public Text textObject;

    private ActionManager actionManager;

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        if (GameObject.Find("DevHint") != null)
        {
            textObject = GameObject.Find("DevHint").transform.GetChild(0).GetComponent<Text>();
        }
    }

    private void Update()
    {
        if (textObject != null)
        {
            textObject.text = actionManager.CurrentDescription;
        }
    }
}
