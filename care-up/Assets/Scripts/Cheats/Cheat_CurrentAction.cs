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
    
    private int direction;
    private float timer;
    
    private ActionManager actionManager;

    private bool set = false; // fix

    void Start()
    {
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        if (GameObject.Find("DevHint") != null)
        {
            if (GameObject.Find("Preferences") != null)
            {
                if (GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode &&
                    actionManager.GetComponent<TutorialManager>() == null)
                {
                    textObject = GameObject.Find("DevHint").transform.GetChild(0).GetComponent<Text>();
                    set = false;
                }
                else
                {
                    GameObject.Find("DevHint").SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Game needs to be started from menu scene for CurrentAction hint to work correctly");
                textObject = GameObject.Find("DevHint").transform.GetChild(0).GetComponent<Text>();
                set = false;
            }
        }
        
        timer = 0.0f;
        direction = 0;
    }

    private void Update()
    {
        if (textObject == null)
            return;

        if (!set)
        {
            textObject.text = actionManager.CurrentDescription;
            set = true;
        }

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
