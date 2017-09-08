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

    public float animationTime = 2.0f;

    // Check only at start of the script
    public bool stepDescrEnabled = true;

    private int direction;
    private float timer;
    
    private ActionManager actionManager;

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        if (GameObject.Find("DevHint") != null)
        {
            if (stepDescrEnabled)
            {
                textObject = GameObject.Find("DevHint").transform.GetChild(0).GetComponent<Text>();
                if (textObject != null)
                {
                    textObject.text = actionManager.CurrentDescription;
                }
            }
            else
            {
                GameObject.Find("DevHint").SetActive(false);
            }
        }
        
        timer = 0.0f;
        direction = 0;
    }

    private void Update()
    {
        if (textObject == null)
            return;

        if ( direction == 1 )
        {
            if ( timer < animationTime )
            {
                timer += Time.deltaTime;
                textObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - timer / animationTime);
            }
            else
            {
                textObject.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                textObject.text = actionManager.CurrentDescription;
                timer = animationTime;
                direction = -1;
            }
        }
        else if (direction == -1)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                textObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - timer / animationTime);
            }
            else
            {
                textObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                timer = 0.0f;
                direction = 0;
            }
        }
    }

    public void UpdateAction()
    {
        direction = 1;
    }
}
