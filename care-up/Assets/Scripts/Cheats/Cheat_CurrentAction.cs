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
    private Text textObjectDevHint;
    private Text textObjectBiggerDevHint;
    private Text extraText;
    private GameObject extraPanel;

    [SerializeField] private GameObject dev_Hint;
    [SerializeField] private GameObject bigger_DevHint;

    private bool biggerDevHintActive = false;
    private bool devHintActive = true;

    private float animationTime = 1.0f;
    
    private int direction;
    private float timer;
	Button extraButton;
    
    private ActionManager actionManager;

    private bool set = false; // fix

    [HideInInspector]
    public bool tutorial_extraOpened = false;
    [HideInInspector]
    public bool tutorial_extraClosed = false;

    Tutorial_UI tutorial_UI;

    void Start()
    {
		extraPanel = GameObject.Find("Extra").gameObject;
        actionManager = GameObject.Find("GameLogic").GetComponent<ActionManager>();
        if (actionManager == null) Debug.LogError("No action manager found.");

        if (GameObject.Find("DevHint") != null)
        {
			Init();

            if (GameObject.Find("Preferences") != null)
            {
            
				if ((!GameObject.Find("Preferences").GetComponent<PlayerPrefsManager>().practiceMode &&
				     actionManager.GetComponent<TutorialManager>() == null) || (FindObjectOfType<TutorialManager>() != null && FindObjectOfType<Tutorial_UI>() == null))
			
                {
                    GameObject.Find("DevHint").SetActive(false);
					extraPanel.SetActive(false);
					extraButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Game needs to be started from menu scene for CurrentAction hint to work correctly");
            }
        }

        if (GameObject.Find ("BiggerDevHint") != null) 
        {
            Init ();

            if (GameObject.Find ("Preferences") != null) 
            {

                if ((!GameObject.Find ("Preferences").GetComponent<PlayerPrefsManager> ().practiceMode &&
                    actionManager.GetComponent<TutorialManager> () == null) || (FindObjectOfType<TutorialManager> () != null && FindObjectOfType<Tutorial_UI> () == null)) 
                
                {
                    GameObject.Find ("BiggerDevHint").SetActive (false);
                    extraPanel.SetActive (false);
                    extraButton.gameObject.SetActive (false);
                }
            } 
            else 
            {
                Debug.LogWarning ("Game needs to be started from menu scene for CurrentAction hint to work correctly");
            }
        }


        timer = 0.0f;
        direction = 0;

        tutorial_UI = GameObject.FindObjectOfType<Tutorial_UI>();
    }

    private void Init()
    {
        GameObject devHint = GameObject.Find("DevHint");
        GameObject biggerDevHint = GameObject.Find ("BiggerDevHint");
        textObjectDevHint = devHint.transform.GetChild(0).GetComponent<Text>();
        textObjectBiggerDevHint = biggerDevHint.transform.GetChild (2).GetComponent<Text> ();

        biggerDevHint.SetActive (false);

        extraText = extraPanel.transform.GetChild(0).GetComponent<Text>();
        extraPanel.SetActive(false);
        set = false;

		extraButton = GameObject.Find("ExtraButton").GetComponent<Button>();
        extraButton.onClick.AddListener(ToggleExtraInfoPanel);

        Button extraCloseBtn = extraPanel.transform.Find("CloseExtra").GetComponent<Button>();
        Button extra_Close_Btn = extraPanel.transform.Find ("CloseExtraCheckmark").GetComponent<Button> ();
        extraCloseBtn.onClick.AddListener(ToggleExtraInfoPanel);
        extra_Close_Btn.onClick.AddListener (ToggleExtraInfoPanel);
    }

    private void Update()
    {

        if (textObjectDevHint == null)
            return;

        if (textObjectBiggerDevHint == null)
            return;

        if (!set) {
            textObjectDevHint.text = actionManager.CurrentDescription;
            textObjectBiggerDevHint.text = actionManager.CurrentDescription;
            extraText.text = actionManager.CurrentExtraDescription;
            set = true;
        }

        if (direction == 1) {
            if (timer < animationTime) {
                timer += Time.deltaTime;
                textObjectDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                textObjectBiggerDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color (0.0f, 0.0f, 0.0f, 0.0f - timer / animationTime);
            } else {
                textObjectDevHint.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
                textObjectBiggerDevHint.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
                extraText.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
                textObjectDevHint.text = actionManager.CurrentDescription;
                textObjectBiggerDevHint.text = actionManager.CurrentDescription;
                extraText.text = actionManager.CurrentExtraDescription;
                timer = animationTime;
                direction = -1;
            }
        } else if (direction == -1) {
            if (timer > 0.0f) {
                timer -= Time.deltaTime;
                textObjectDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                textObjectBiggerDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
                extraText.color = new Color (0.0f, 0.0f, 0.0f, 1.0f - timer / animationTime);
            } else {
                textObjectDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f);
                textObjectBiggerDevHint.color = new Color (0.0f, 0.0f, 0.0f, 1.0f);
                extraText.color = new Color (0.0f, 0.0f, 0.0f, 1.0f);
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

    public void ShowBiggerDevHint () {
        dev_Hint.SetActive (false);
        bigger_DevHint.SetActive (true);

        biggerDevHintActive = true;
        devHintActive = false;
    }

    public void RemoveBiggerDevHint () {
        dev_Hint.SetActive (true);
        bigger_DevHint.SetActive (false);

        biggerDevHintActive = false;
        devHintActive = true;
    }

    public void RemoveDevHint () {
        dev_Hint.SetActive (false);
    }

    public void ShowDevHint () {
        dev_Hint.SetActive (true);
    }
}
