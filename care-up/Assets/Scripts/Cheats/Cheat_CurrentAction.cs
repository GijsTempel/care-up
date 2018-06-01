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
    private Text textObject;
    private Text extraText;
    private GameObject extraPanel;

    private float animationTime = 1.0f;
    
    private int direction;
    private float timer;
    
    private ActionManager actionManager;

    private bool set = false; // fix

    [HideInInspector]
    public bool tutorial_extraOpened = false;
    [HideInInspector]
    public bool tutorial_extraClosed = false;

    Tutorial_UI tutorial_UI;

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
                    Init();
                }
                else
                {
                    GameObject.Find("DevHint").SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Game needs to be started from menu scene for CurrentAction hint to work correctly");
                Init();
            }
        }
        
        timer = 0.0f;
        direction = 0;

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
    }

    private void Init()
    {
        GameObject devHint = GameObject.Find("DevHint");
        textObject = devHint.transform.GetChild(0).GetComponent<Text>();
        extraPanel = devHint.transform.Find("Extra").gameObject;
        extraText = extraPanel.transform.GetChild(0).GetComponent<Text>();
        extraPanel.SetActive(false);
        set = false;

        Button extraButton = devHint.transform.Find("ExtraButton").GetComponent<Button>();
        extraButton.onClick.AddListener(ToggleExtraInfoPanel);
    }

    private void Update()
    {
        if (textObject == null)
            return;

        if (!set)
        {
            textObject.text = actionManager.CurrentDescription;
            extraText.text = actionManager.CurrentExtraDescription;
            set = true;
        }

        if ( direction == 1 )
        {
            if ( timer < animationTime )
            {
                timer += Time.deltaTime;
                textObject.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - timer / animationTime);
            }
            else
            {
                textObject.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                extraText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                textObject.text = actionManager.CurrentDescription;
                extraText.text = actionManager.CurrentExtraDescription;
                timer = animationTime;
                direction = -1;
            }
        }
        else if (direction == -1)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                textObject.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - timer / animationTime);
            }
            else
            {
                textObject.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                extraText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                timer = 0.0f;
                direction = 0;
            }
        }
    }

    public void UpdateAction()
    {
        direction = 1;
        if (extraPanel != null)
        {
            extraPanel.SetActive(false);
        }
    }

    public void ToggleExtraInfoPanel()
    {
        if (tutorial_UI != null && tutorial_UI.expectedHintsState == extraPanel.activeSelf)
        {
            return;
        }

        extraPanel.SetActive(!extraPanel.activeSelf);

        if (extraPanel.activeSelf)
        {
            tutorial_extraOpened = true;
        }
        else
        {
            tutorial_extraClosed = true;
        }
    }
}
